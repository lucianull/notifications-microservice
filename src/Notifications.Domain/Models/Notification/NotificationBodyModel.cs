using Notifications.Domain.Enums;

namespace Notifications.Domain.Models.Notification;

public class NotificationBodyModel
{
    private NotificationTypeTagEnum notificationTypeTag;

    public NotificationTypeTagEnum NotificationTypeTag
    {
        get => notificationTypeTag;
        set => notificationTypeTag = value;
    }
}
