using System.Text;
using backend.Data;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using backend.Data.Models;
using backend.Hubs;
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
builder.Services.AddScoped<SoftDeleteInterceptor>();
builder.Services.AddScoped<NormalizationInterceptor>();

// ── Lexicon rules (پڕۆمپت ٦) ───────────────────────────────────────────────
// Service layer, not UI: a rule enforced in a Razor component is enforced only for people who go
// through that component, and the API, imports and bulk edits all bypass it.
// The options tree lives in a process-wide cache with a change notification, so a settings write
// reaches every open session immediately instead of on the next page load (پڕۆمپت — ڕیاڵ تایم).
builder.Services.AddSingleton<backend.Services.Lexicon.TaxonomyCache>();
builder.Services.AddScoped<TaxonomyChangeInterceptor>();
builder.Services.AddScoped<backend.Services.Lexicon.OptionsTreeService>();

builder.Services.AddScoped<backend.Services.Lexicon.LexiconValidator>();
builder.Services.AddScoped<backend.Services.Lexicon.RelationService>();
builder.Services.AddScoped<backend.Services.Lexicon.WorkQueueService>();
builder.Services.AddScoped<backend.Services.Lexicon.ClaimService>();
builder.Services.AddScoped<backend.Services.Lexicon.ClassificationService>();
builder.Services.AddScoped<backend.Services.Lexicon.ContributorCreditService>();
builder.Services.AddScoped<backend.Services.Lexicon.TaxonomyAdminService>();
builder.Services.AddScoped<backend.Services.Lexicon.TaxonomyTreeService>();
builder.Services.AddScoped<backend.Services.Lexicon.MergeService>();
builder.Services.AddScoped<backend.Services.Lexicon.PartOfSpeechReassignService>();
builder.Services.AddScoped<backend.Services.Lexicon.StationService>();
builder.Services.AddScoped<ContributionEventInterceptor>();

// Pushes every audited write to the admin clients — AuditSaveChangesInterceptor broadcasts through
// IActivityBroadcaster the moment its rows commit. Singleton, so a scoped interceptor can hold it.
builder.Services.AddSignalR();
builder.Services.AddSingleton<IActivityBroadcaster, ActivityBroadcaster>();

// Pushes taxonomy changes to every open circuit. Hosted rather than called, so no settings endpoint
// can be added next year that forgets to announce itself.
builder.Services.AddHostedService<backend.Hubs.TaxonomyChangeBroadcaster>();

// Profile pictures. Singleton because it only holds a configured folder path and creates it once.
builder.Services.AddSingleton<AvatarService>();

builder.Services.AddHttpClient<IGeoLocationService, GeoLocationService>(client =>
{
    // A slow geo provider must not hold up an analytics write.
    client.Timeout = TimeSpan.FromSeconds(3);
});

builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

    // Order matters. SoftDelete runs first and rewrites Deleted → Modified, so the ledger and the
    // audit log both see a soft delete rather than a hard one. Registering it after them would let
    // a DELETE be recorded that never actually happens.
    options.AddInterceptors(
        // First: derive Normalized, so the ledger and the audit log both record the finished row.
        sp.GetRequiredService<NormalizationInterceptor>(),
        sp.GetRequiredService<SoftDeleteInterceptor>(),
        sp.GetRequiredService<ContributionEventInterceptor>(),
        sp.GetRequiredService<AuditSaveChangesInterceptor>(),

        // Last: the cache drop runs on SavedChanges, after the transaction has committed. Dropping
        // it earlier would let a concurrent read re-cache the rows that are about to change.
        sp.GetRequiredService<TaxonomyChangeInterceptor>());
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

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // A browser cannot set headers on a WebSocket handshake, so SignalR falls back to
                // passing the token in the query string. Accepted only on the hub path — anywhere
                // else it would put credentials into request logs and referrers.
                var token = context.Request.Query["access_token"];

                if (!string.IsNullOrEmpty(token) &&
                    context.HttpContext.Request.Path.StartsWithSegments("/hubs"))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            },
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

// ── Uploaded avatars ───────────────────────────────────────────────────────
// Served from a configured folder that is NOT wwwroot, and mapped to its own request path.
//
// Two things make this safe to expose without auth. The folder holds nothing but images this
// application itself encoded — AvatarService discards the uploaded name and re-encodes the
// bytes — and the content type is pinned to image/jpeg rather than sniffed, so a file that
// somehow was not a JPEG still cannot be served as script. A profile picture is shown to every
// signed-in user anyway; gating it would buy nothing and cost a round trip per avatar.
{
    var avatars = app.Services.GetRequiredService<AvatarService>();

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(avatars.Folder),
        RequestPath = "/avatars",
        ContentTypeProvider = new FileExtensionContentTypeProvider(
            new Dictionary<string, string> { [".jpg"] = "image/jpeg" }),

        // Anything without a mapped extension is not served at all, rather than being handed
        // over as application/octet-stream.
        ServeUnknownFileTypes = false,

        OnPrepareResponse = ctx =>
        {
            // Names are content-addressed by Guid and a new upload gets a new name, so a long
            // cache is safe and saves a request per avatar per page.
            ctx.Context.Response.Headers.CacheControl = "public,max-age=31536000,immutable";
            ctx.Context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        },
    });
}

// HTTPS handled by Cloudflare — no redirect needed on origin
app.UseCors(PublicSiteCors);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ActivityHub>("/hubs/activity");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

await DbSeeder.SeedAsync(app.Services, app.Configuration, app.Logger);

app.Run();
