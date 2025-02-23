namespace Notifications.Domain.Models
{
    public class ReceiverNotificationLog
    {
        private string receiverId = string.Empty;
        private DateTime sentAt;

        public string ReceiverId
        {
            get => receiverId;
            set => receiverId = value;
        }

        public DateTime SentAt
        {
            get => sentAt;
            set => sentAt = value;
        }
    }
}