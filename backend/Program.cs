using System.Text;
using backend.Data;
using backend.Data.Models;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    // Lets you paste a token into Swagger and exercise the protected endpoints.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Bearer token. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ── Auth configuration ────────────────────────────────────────────────────
var jwt = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();

if (string.IsNullOrWhiteSpace(jwt.Key))
{
    // A signing key that ships in source control is not a signing key. Refuse to start in
    // production rather than sign tokens anyone can forge; generate a throwaway one in dev.
    if (builder.Environment.IsProduction())
        throw new InvalidOperationException(
            "Jwt:Key is not configured. Set the Jwt__Key environment variable to a random 32+ character secret.");

    jwt.Key = "dev-only-insecure-signing-key-change-me-please-32+";
}

builder.Services.AddSingleton(jwt);
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<AuditSaveChangesInterceptor>();

builder.Services.AddHttpClient<IGeoLocationService, GeoLocationService>(client =>
{
    // A slow geo provider must not hold up an analytics write.
    client.Timeout = TimeSpan.FromSeconds(3);
});

builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>());
});

builder.Services
    .AddIdentityCore<AppUser>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = true;

        options.User.RequireUniqueEmail = true;

        // Slow down password guessing: five misses locks the account for fifteen minutes.
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    })
    .AddRoles<AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            // Default is 5 minutes of leeway; a token is expired when it says it is.
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();

// The Next.js site posts analytics events from the visitor's browser, so it needs CORS.
const string PublicSiteCors = "public-site";
builder.Services.AddCors(options =>
{
    var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                  ?? new[] { "http://localhost:4000", "http://localhost:6001" };

    options.AddPolicy(PublicSiteCors, policy => policy
        .WithOrigins(origins)
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS handled by Cloudflare — no redirect needed on origin
app.UseCors(PublicSiteCors);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

await DbSeeder.SeedAsync(app.Services, app.Configuration, app.Logger);

app.Run();
