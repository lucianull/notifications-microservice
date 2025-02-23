namespace Notifications.Domain.Entities
{
    public class User
    {
        private string? id;
        private int authId;

        public string? Id
        {
            get => id;
            set => id = value;
        }

        public int AuthId
        {
            get => authId;
            set => authId = value;
        }
    }
}