using System.ComponentModel.DataAnnotations;

namespace Notifications.Domain.Models.Event
{
    public class EventCreateModel
    {
        private List<ReceiverModel> receivers = new();
        private EventBodyModel data = new EventBodyModel();

        public List<ReceiverModel> Receivers
        {
            get => receivers;
            set => receivers = value;
        }

        [Required]
        public EventBodyModel Data
        {
            get => data;
            set => data = value;
        }
    }
}