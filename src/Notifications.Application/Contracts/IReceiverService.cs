using Notifications.Domain.Entities;
using Notifications.Domain.Models;

namespace Notifications.Application.Contracts;

public interface IReceiverService
{
    Task<List<Receiver>> AddReceiversAsync(List<ReceiverModel> receivers);
}
