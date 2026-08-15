FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# SkiaSharp, which crops and re-encodes every profile picture, is a NATIVE library, and the build
# in SkiaSharp.NativeAssets.Linux links against fontconfig. The aspnet image is Debian slim and
# does not ship it, so libSkiaSharp cannot be loaded at all: the first SKBitmap.Decode throws
# DllNotFoundException, and the upload endpoint answers with a bare 500. Nothing in the managed
# build is wrong, which is exactly why this is easy to chase in the wrong place — it works on a
# Windows dev machine, where the native asset comes from the Win32 package instead.
RUN apt-get update \
    && apt-get install -y --no-install-recommends libfontconfig1 \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["backend/backend.csproj", "backend/"]
COPY ["shared/Shared.csproj", "shared/"]
RUN dotnet restore "backend/backend.csproj"
COPY backend/ backend/
COPY shared/ shared/
RUN dotnet build "backend/backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "backend/backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "backend.dll"]
