namespace Notifications.Domain.Models.Event;

public class ActivateAccountModel : EventBodyModel
{
    private string firstName = string.Empty;
    private string lastName = string.Empty;
    private string activationLink = string.Empty;

    public string FirstName
    {
        get => firstName;
        set => firstName = value;
    }

    public string LastName
    {
        get => lastName;
        set => lastName = value;
    }
    
    public string ActivationLink
    {
        get => activationLink;
        set => activationLink = value;
    }
}
