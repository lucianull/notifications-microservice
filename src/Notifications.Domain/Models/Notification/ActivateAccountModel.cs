namespace Notifications.Domain.Models.Notification;

public class ActivateAccountModel : NotificationBodyModel
{
    private string activationLink = string.Empty;

    public string ActivationLink
    {
        get => activationLink;
        set => activationLink = value;
    }
}
