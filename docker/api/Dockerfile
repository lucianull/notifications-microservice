FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy the csproj and restore dependencies
COPY ["src/Notifications.Api/Notifications.Api.csproj", "src/Notifications.Api/"]
COPY ["src/Notifications.Application/Notifications.Application.csproj", "src/Notifications.Application/"]
COPY ["src/Notifications.Domain/Notifications.Domain.csproj", "src/Notifications.Domain/"]
COPY ["src/Notifications.Infrastructure/Notifications.Infrastructure.csproj", "src/Notifications.Infrastructure/"]

RUN dotnet restore "src/Notifications.Api/Notifications.Api.csproj"

# Copy the rest of the code
COPY . .

# Build and publish the app
RUN dotnet publish "src/Notifications.Api/Notifications.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# EXPOSE 9000

# Copy the published app from the build stage
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Notifications.Api.dll"]
