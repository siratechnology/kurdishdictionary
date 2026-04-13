FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

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
