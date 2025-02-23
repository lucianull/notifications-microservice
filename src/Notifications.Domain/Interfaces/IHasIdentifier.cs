namespace Notifications.Domain.Interfaces;

public interface IHasIdentifier
{
    string Identifier { get; set; }
    int Counter { get; set; }
}
