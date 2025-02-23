using DotNetEnv;
using Notifications.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AutoMapper;
using System.Reflection;
using Notifications.Application.Strategies.NotificationStrategies;
using Notifications.Infrastructure.Repositories;
using Notifications.Application.Contracts;
using Notifications.Application.Services;
using Notifications.Domain.Serializer;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from the correct location
var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), "../../.env");

Env.Load(envFilePath); // Load .env from the root folder

var aspNetCoreUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
Console.WriteLine($"ASP.NET Core URLs: {aspNetCoreUrls}"); // Check if this is being loaded correctly

// Configure the server to listen on port 9000
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(9000); // Ensure Kestrel listens on port 9000
});

builder.WebHost.UseUrls(aspNetCoreUrls); // Use the environment variable to configure the server URL

// Register RavenDB as a singleton
builder.Services.AddSingleton<RavenDbContext>(sp =>
{
    var ravenDbUrl = Environment.GetEnvironmentVariable("RAVENDB_URLS");
    var ravenDbDatabase = Environment.GetEnvironmentVariable("RAVENDB_DATABASE");
    if (string.IsNullOrEmpty(ravenDbUrl) || string.IsNullOrEmpty(ravenDbDatabase))
    {
        throw new InvalidOperationException("RavenDB URL or Database name is not set");
    }
    Console.WriteLine($"RavenDB URL: {ravenDbUrl}");
    return new RavenDbContext(ravenDbUrl, ravenDbDatabase);
});

// Register repositories
builder.Services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
builder.Services.AddScoped<IReceiverRepository, ReceiverRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// Add AutoMapper and scan for all profiles in the assembly
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(Assembly.GetExecutingAssembly());
}, typeof(Notifications.Application.Mappings.SuppliersPortalModelsProfile).Assembly);


// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        // Add custom Enum converter
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        // options.SerializerSettings.Converters.Add(new EventBodyModelConverter());

        // Optionally, add other settings like formatting
        options.SerializerSettings.Formatting = Formatting.Indented;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<SuppliersPortalActivateAccountStrategy>();

// Register the NotificationStrategyContext and NotificationStrategyFactory
builder.Services.AddScoped<INotificationStrategyContext, NotificationStrategyContext>();
builder.Services.AddSingleton<NotificationFactory>();

// Add the notification strategies
builder.Services.AddNotifications();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();