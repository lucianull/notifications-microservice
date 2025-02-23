using Notifications.Domain.Entities;
using Raven.Client.Documents.Indexes;

namespace Notifications.Infrastructure.Indexes;

public class Notification_ByTypeAndGroup : AbstractIndexCreationTask<Notification>
{
    public class IndexEntry
    {
        public string NotificationTypeId { get; set; }
        public string GroupId { get; set; }
    }

    public Notification_ByTypeAndGroup()
    {
        Map = notifications => from notification in notifications
                               select new IndexEntry
                               {
                                   NotificationTypeId = notification.NotificationTypeId,
                                   GroupId = notification.GroupId,
                               };

        // Optional: You can store the fields if you need them for projections, etc.
        Stores.Add(x => x.NotificationTypeId, FieldStorage.Yes);
        Stores.Add(x => x.GroupId, FieldStorage.Yes);
    }
}