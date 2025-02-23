using Notifications.Domain.Entities;
using Notifications.Domain.Enums;
using Notifications.Infrastructure.Indexes;
using Raven.Client.Documents;

namespace Notifications.Infrastructure.Repositories
{
    public class NotificationTypeRepository : INotificationTypeRepository
    {
        private readonly RavenDbContext _ravenDbContext;

        public NotificationTypeRepository(RavenDbContext ravenDbContext)
        {
            _ravenDbContext = ravenDbContext;
        }

        public async Task<NotificationType> GetNotificationTypeByTagAsync(NotificationTypeTagEnum tag)
        {
            // Open an asynchronous session using RavenDbContext's store
            using var session = _ravenDbContext.Store.OpenAsyncSession();

            // Load the NotificationType by its id (which is the Tag as a string)
            // Note: We assume the Tag is being used as the document id in RavenDB
            NotificationType? notificationType = await session
                            .Query<NotificationType, NotificationType_ByTag>()
                            .FirstOrDefaultAsync(nt => nt.Tag == tag);
            if (notificationType == null)
            {
                throw new Exception($"NotificationType with tag {tag} not found.");
            }
            return notificationType;
        }
    }
}
