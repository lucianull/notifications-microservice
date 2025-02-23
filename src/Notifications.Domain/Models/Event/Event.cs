namespace Notifications.Domain.Models.Event;

public class Event
{
    private string? identifier = null;
    private int? counter;

    private EventBodyModel? data;
    
    public string? Identifier
    {
        get => identifier;
        set => identifier = value;
    }

    public int? Counter
    {
        get => counter;
        set => counter = value;
    }

    public EventBodyModel? Data
    {
        get => data;
        set => data = value;
    }
}