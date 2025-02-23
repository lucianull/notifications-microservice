using Microsoft.Extensions.DependencyInjection;
using Notifications.Application.Strategies.NotificationStrategies;
using Notifications.Domain.Enums;

namespace Notifications.Application.Services;

public class NotificationFactory
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public INotificationStrategy GetStrategy(NotificationTypeTagEnum notificationTypeTag)
    {
        return _serviceProvider.GetRequiredKeyedService<INotificationStrategy>(notificationTypeTag);
    }
}
