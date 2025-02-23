FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore "src/Notifications.Api/Notifications.Api.csproj"

# Build and publish the app
RUN dotnet publish "src/Notifications.Api/Notifications.Api.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
# EXPOSE 9000

# Copy the published app from the build stage
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Notifications.Api.dll"]
