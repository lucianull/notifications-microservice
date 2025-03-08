using Notifications.Domain.Entities;
using Notifications.Domain.Enums;

namespace Notifications.Infrastructure.Repositories;

public interface INotificationRepository
{
    Task SaveAsync(Notification notification);
    Task<Notification?> GetByIdAsync(string id);
    Task<Notification?> GetPendingNotificationByTypeAndGroupIdAsync(NotificationTypeTagEnum type, string groupId);
    Task<Notification> TryCreateNotificationAsyncWithLock(Notification notification);
    Task<Notification> CreateNotificationAsync(Notification notification);

}
