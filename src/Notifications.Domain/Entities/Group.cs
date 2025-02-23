namespace Notifications.Domain.Entities;

public class Group
{
    private string? id;
    private string? hash = null;
    private List<string> receiverIds = new();

    public string? Id
    {
        get => id;
        set => id = value;
    }

    public string? Hash
    {
        get => hash;
        set => hash = value;
    }

    public List<string> ReceiverIds
    {
        get => receiverIds;
        set => receiverIds = value;
    }
}
