using Microsoft.Extensions.DependencyInjection;
using Notifications.Application.Strategies.NotificationStrategies;
using Notifications.Domain.Enums;

namespace Notifications.Application.Services;

public static class NotificationCollectionExtensions
{
    public static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        services.AddTransient<NotificationFactory>();
        var notificationStrategies = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(INotificationStrategy).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
            .ToList();
        
        foreach(var strategy in notificationStrategies)
        {
            NotificationTypeTagEnum type = (NotificationTypeTagEnum) strategy!.GetProperty("Type")!.GetValue(null, null)!;
            services.AddKeyedTransient(typeof(INotificationStrategy), type, strategy);
        }
        return services;
    }
}
