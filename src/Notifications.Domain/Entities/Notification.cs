using Newtonsoft.Json;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;
using Notifications.Domain.Models.Event;
using Notifications.Domain.Models.Notification;

namespace Notifications.Domain.Entities
{
    public class Notification : IHasCompareExchangeKey
    {
        public static readonly int OPTIMISTIC_RETRIES = 3;
        private string? id;
        private string groupId;
        private string notificationTypeId;
        private int? eventsCount;
        private List<Event> events = new();
        private List<ReceiverNotificationLog> receiverLogs = new();

        private NotificationBodyModel? notificationBodyModel = null;

        public string? Id
        {
            get => id;
            set => id = value;
        }

        public string GroupId
        {
            get => groupId;
            set => groupId = value;
        }

        public string NotificationTypeId
        {
            get => notificationTypeId;
            set => notificationTypeId = value;
        }

        public int? EventsCount
        {
            get => eventsCount;
            set => eventsCount = value;
        }

        public List<Event> Events
        {
            get => events;
            set => events = value;
        }

        public List<ReceiverNotificationLog> ReceiverLogs
        {
            get => receiverLogs;
            set => receiverLogs = value;
        }

        public NotificationBodyModel? NotificationBodyModel
        {
            get => notificationBodyModel;
            set => notificationBodyModel = value;
        }

        public string GetCompareExchangeKey()
        {
            if (string.IsNullOrEmpty(GroupId) || string.IsNullOrEmpty(NotificationTypeId))
            {
                throw new InvalidOperationException("GroupId and NotificationTypeId must be set to generate a CompareExchangeKey.");
            }

            return $"notification/{GroupId}/{NotificationTypeId}";
        }

        public void AddOrUpdateEvent(Event newEvent)
        {
            // Check if 'Data' implements IHasIdentifier
            if (newEvent.Data is IHasIdentifier identifierData)
            {
                // If it implements IHasIdentifier, use the identifier from Data
                string eventIdentifier = identifierData.Identifier;

                // Search for the event in the collection using the identifier
                var existingEvent = Events.FirstOrDefault(e => e.Identifier == eventIdentifier);

                if (existingEvent != null)
                {
                    // If the event already exists, increment the counter
                    existingEvent.Counter = (existingEvent.Counter ?? 0) + 1;
                }
                else
                {
                    // If the event does not exist, add a new event
                    Events.Add(newEvent);
                    EventsCount = (EventsCount ?? 0) + 1;
                }
            }
            else
            {
                // If 'Data' does not implement IHasIdentifier, add the event without checking
                Events.Add(newEvent);
                EventsCount = (EventsCount ?? 0) + 1;
            }
        }
    }
}