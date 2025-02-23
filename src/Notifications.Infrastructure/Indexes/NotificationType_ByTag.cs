using Notifications.Domain.Entities;
using Raven.Client.Documents.Indexes;

namespace Notifications.Infrastructure.Indexes;

public class NotificationType_ByTag : AbstractIndexCreationTask<NotificationType>
{
    public NotificationType_ByTag()
    {
        Map = notificationTypes => from nt in notificationTypes
                                   select new { nt.Tag };

        Index(n => n.Tag, FieldIndexing.Default);
    }
}
