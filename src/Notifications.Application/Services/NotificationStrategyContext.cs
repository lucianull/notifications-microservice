using Notifications.Application.Contracts;
using Notifications.Domain.Models.Event;

namespace Notifications.Application.Services;

public class NotificationStrategyContext : INotificationStrategyContext
{
    NotificationFactory _notificationFactory;
    public NotificationStrategyContext(
        NotificationFactory notificationFactory
    )
    {
        _notificationFactory = notificationFactory;
    }
    public async Task createEventAsync(EventCreateModel eventCreateModel)
    {
        await _notificationFactory.GetStrategy(eventCreateModel.Data.NotificationTypeTag).createEventAsync(eventCreateModel);
    }
}
