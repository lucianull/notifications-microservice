using System.ComponentModel.DataAnnotations;

namespace Notifications.Domain.Models;

public class ReceiverModel
{
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    private string? email = null;
    [Phone(ErrorMessage = "Invalid phone number.")]
    private string? phone = null;
    [Required(ErrorMessage = "First name is required.")]
    private string? firstName = null;
    [Required(ErrorMessage = "Last name is required.")]
    private string? lastName = null;

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
