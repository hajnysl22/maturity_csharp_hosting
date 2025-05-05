# Use official .NET SDK image to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore dependencies
COPY . ./
RUN dotnet restore "PoznamkyApp/PoznamkyApp.sln"

# Build the app
RUN dotnet publish "PoznamkyApp/PoznamkyApp.sln" -c Release -o /app/publish

# Final stage: runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "PoznamkyApp.dll"]
