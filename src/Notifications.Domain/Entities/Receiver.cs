namespace Notifications.Domain.Entities;

public class Receiver
{
    private string? id;
    private string? email = null;
    private string? phone = null;
    private string? firstName = null;
    private string? lastName = null;

    public string? Id
    {
        get => id;
        set => id = value;
    }

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

    public string? FirstName
    {
        get => firstName;
        set => firstName = value;
    }

    public string? LastName
    {
        get => lastName;
        set => lastName = value;
    }
}