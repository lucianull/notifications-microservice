using System.ComponentModel.DataAnnotations;

namespace Notifications.Domain.Models.Event
{
    public class EventCreateModel
    {
        private List<string> receiverEmails = new List<string>();
        private List<string> receiverPhones = new List<string>();
        private EventBodyModel data = new EventBodyModel();

        [Required(ErrorMessage = "Receiver emails should not be null.")]
        // [EmailAddress(ErrorMessage = "The email {0} is not a valid email.")]
        public List<string> ReceiverEmails
        {
            get => receiverEmails;
            set => receiverEmails = value;
        }

        [Required(ErrorMessage = "Receiver phones should not be null.")]
        // [RegularExpression(@"^\+[1-9]\d{1,14}$", ErrorMessage = "The phone number {0} is not a valid international phone number.")]
        public List<string> ReceiverPhones
        {
            get => receiverPhones;
            set => receiverPhones = value;
        }

        [Required]
        public EventBodyModel Data
        {
            get => data;
            set => data = value;
        }
    }
}