using Raven.Client.Documents;
using Notifications.Domain.Entities;
using Notifications.Domain.Enums;

namespace Notifications.Infrastructure;

public static class RavenDbSeeder
{
    public static async Task SeedNotificationTypesAsync(IDocumentStore store)
    {
        using (var session = store.OpenAsyncSession())
        {
            // Check if the NotificationType documents already exist
            var existingTypes = await session.Query<NotificationType>().ToListAsync();
            if (existingTypes.Count == 0)
            {
                // Add initial NotificationType documents
                var notificationTypes = new[]
                {
                        new NotificationType { 
                            Tag = NotificationTypeTagEnum.SUPPLIERS_PORTAL_ACTIVATE_ACCOUNT,
                            ClientId = "suppliersPortal",
                            Type = NotificationTypeEnum.EMAIL,
                            Subject = "Activate your account",
                            Template = "Hello, please click the link below to activate your account",
                            TimeFrame = 0,
                            Threshold = 1
                        },
                    };

                foreach (var notificationType in notificationTypes)
                {
                    await session.StoreAsync(notificationType);
                }

                await session.SaveChangesAsync();
            }
        }
    }
}
