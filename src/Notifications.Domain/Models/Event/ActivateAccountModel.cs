namespace Notifications.Domain.Models.Event;

public class ActivateAccountModel : EventBodyModel
{
    private string activationLink = string.Empty;
    
    public string ActivationLink
    {
        get => activationLink;
        set => activationLink = value;
    }
}
