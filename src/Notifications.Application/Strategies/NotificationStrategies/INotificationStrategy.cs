using Notifications.Domain.Entities;
using Notifications.Domain.Enums;
using Notifications.Domain.Models.Event;
using Notifications.Domain.Models.Notification;

namespace Notifications.Application.Strategies.NotificationStrategies;

public interface INotificationStrategy
{
    static NotificationTypeTagEnum Type { get; }
    Task createEventAsync(EventCreateModel eventCreateModel);
    Task <NotificationBodyModel> createNotificationBodyObjectAsync(Notification eventCreateModel);
}
