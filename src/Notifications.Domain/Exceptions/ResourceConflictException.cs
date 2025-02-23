namespace Notifications.Domain.Exceptions;

public class ResourceConflictException : Exception
{
    public string ResourceName { get; }
    public string UniqueKey { get; }

    public ResourceConflictException(string resourceName, string uniqueKey)
        : base($"Conflict occurred with resource '{resourceName}' using unique key '{uniqueKey}'.")
    {
        ResourceName = resourceName;
        UniqueKey = uniqueKey;
    }

    public ResourceConflictException(string resourceName, string uniqueKey, string message)
        : base(message)
    {
        ResourceName = resourceName;
        UniqueKey = uniqueKey;
    }
}

