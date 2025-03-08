namespace Notifications.Domain.Models;

public class EmailPhonePair
{
    private string? email;
    private string? phone;

    public string? Email
    {
        get => email;
        set => email = value;
    }

    public string? Phone
    {
        get => phone;
        set => phone = value;
    }
}
