using Notifications.Domain.Entities;
using Notifications.Domain.Enums;

namespace Notifications.Infrastructure.Repositories;

/// <summary>
/// Interface for the NotificationType repository.
/// </summary>
public interface INotificationTypeRepository
{
    /// <summary>
    /// Gets a NotificationType by its tag.
    /// </summary>
    /// <param name="tag">The tag of the NotificationType to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the NotificationType with the specified tag.</returns>
    Task<NotificationType> GetNotificationTypeByTagAsync(NotificationTypeTagEnum tag);

}
