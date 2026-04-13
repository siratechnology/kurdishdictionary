FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# ── Stage 1: Build Tailwind CSS ──────────────────────────────────────────────
FROM node:20-alpine AS css-builder
WORKDIR /app
COPY frontend-blazor/package.json frontend-blazor/package-lock.json* ./
RUN npm ci
# Copy all razor/html files so Tailwind can scan for used classes
COPY frontend-blazor/ .
RUN mkdir -p wwwroot/css && npm run build:css

# ── Stage 2: Build .NET app ───────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["frontend-blazor/frontend-blazor.csproj", "frontend-blazor/"]
COPY ["shared/Shared.csproj", "shared/"]
RUN dotnet restore "frontend-blazor/frontend-blazor.csproj"
COPY frontend-blazor/ frontend-blazor/
COPY shared/ shared/
# Bring in the compiled CSS from the Node stage
COPY --from=css-builder /app/wwwroot/css frontend-blazor/wwwroot/css/
# Skip the npm MSBuild targets — CSS is already compiled above
RUN dotnet build "frontend-blazor/frontend-blazor.csproj" -c Release -o /app/build /p:SkipNpmBuild=true

# ── Stage 3: Publish ──────────────────────────────────────────────────────────
FROM build AS publish
RUN dotnet publish "frontend-blazor/frontend-blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:SkipNpmBuild=true

# ── Stage 4: Final runtime image ─────────────────────────────────────────────
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "frontend-blazor.dll"]
