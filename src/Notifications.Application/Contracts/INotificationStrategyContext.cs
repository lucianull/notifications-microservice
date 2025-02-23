using Notifications.Domain.Models.Event;

namespace Notifications.Application.Contracts;

public interface INotificationStrategyContext
{
    Task createEventAsync(EventCreateModel eventCreateModel);
}
