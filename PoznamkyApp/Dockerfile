FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything from the repo
COPY . .

# Restore using full relative path
RUN dotnet restore "./PoznamkyApp/PoznamkyApp.sln"

# Publish
RUN dotnet publish "./PoznamkyApp/PoznamkyApp.sln" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PoznamkyApp.dll"]
