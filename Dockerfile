# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY LeagueApi/LeagueApi.csproj LeagueApi/
RUN dotnet restore LeagueApi/LeagueApi.csproj

# Copy everything else and build
COPY . .
WORKDIR /src/LeagueApi
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "LeagueApi.dll"]