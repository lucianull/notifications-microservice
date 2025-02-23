using Newtonsoft.Json;
using Notifications.Domain.Enums;
using Notifications.Domain.Serializer;

namespace Notifications.Domain.Entities
{
    public class NotificationType
    {
        private string? id;
        [JsonConverter(typeof(EnumToStringConverter))]
        private NotificationTypeTagEnum tag;
        private string? description = string.Empty;
        private string? clientId;
        [JsonConverter(typeof(EnumToStringConverter))]
        private NotificationTypeEnum type;
        private string? subject;
        private string? template;
        private int timeFrame;
        private int threshold;

        public string? Id
        {
            get => id;
            set => id = value;
        }
        public NotificationTypeTagEnum Tag
        {
            get => tag;
            set => tag = value;
        }

        public string? Description
        {
            get => description;
            set => description = value;
        }

        public string? ClientId
        {
            get => clientId;
            set => clientId = value;
        }

        public NotificationTypeEnum Type
        {
            get => type;
            set => type = value;
        }

        public string? Subject
        {
            get => subject;
            set => subject = value;
        }

        public string? Template
        {
            get => template;
            set => template = value;
        }

        public int TimeFrame
        {
            get => timeFrame;
            set => timeFrame = value;
        }

        public int Threshold
        {
            get => threshold;
            set => threshold = value;
        }
    }
}