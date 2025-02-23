using Raven.Client.Documents;
using Notifications.Domain.Entities;
using Notifications.Infrastructure.Indexes;
using Notifications.Domain.Enums;
using Notifications.Domain.Exceptions;
using Raven.Client.Documents.Operations.CompareExchange;

namespace Notifications.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly RavenDbContext _ravenDbContext;

    public NotificationRepository(RavenDbContext ravenDbContext)
    {
        _ravenDbContext = ravenDbContext;
    }

    public async Task SaveAsync(Notification notification)
    {
        using var session = _ravenDbContext.Store.OpenAsyncSession();
        await session.StoreAsync(notification);
        await session.SaveChangesAsync();
    }

    public async Task<Notification?> GetByIdAsync(string id)
    {
        using var session = _ravenDbContext.Store.OpenAsyncSession();
        return await session.LoadAsync<Notification>(id);
    }

    public async Task<Notification?> GetPendingNotificationByTypeAndGroupIdAsync(NotificationTypeTagEnum type, string groupId)
    {
        using var session = _ravenDbContext.Store.OpenAsyncSession();
        return await session.Query<Notification, Notification_ByTypeAndGroup>()
                            .Where(n => n.NotificationTypeId == type.ToString() && n.GroupId == groupId)
                            .FirstOrDefaultAsync();
    }

    public async Task<Notification> TryCreateNotificationAsync(Notification notification)
    {
        using (var session = _ravenDbContext.Store.OpenAsyncSession())
        {
            string uniqueKey = notification.GetCompareExchangeKey();

            // Try to acquire Compare Exchange lock
            var result = await _ravenDbContext.Store.Operations
                .SendAsync(new PutCompareExchangeValueOperation<string>(uniqueKey, "locked", 0));

            if (!result.Successful)
            {
                // 🔹 Retrieve existing notification ID before throwing exception
                var existingNotification = await session.Query<Notification>()
                    .Where(n => n.GroupId == notification.GroupId && n.NotificationTypeId == notification.NotificationTypeId)
                    .FirstOrDefaultAsync();

                if (existingNotification == null)
                {
                    throw new Exception("Failed to acquire Compare Exchange lock, but no existing notification found.");
                }                

                throw new ResourceConflictException($"Notification with ID {existingNotification.Id} already exists.", existingNotification.Id);
            }

            await session.StoreAsync(notification);
            await session.SaveChangesAsync();
            return notification;
        }
    }

}
