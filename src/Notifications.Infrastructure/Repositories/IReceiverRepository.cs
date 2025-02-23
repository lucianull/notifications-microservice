using Notifications.Domain.Entities;

namespace Notifications.Application.Contracts
{
    public interface IReceiverRepository
    {
        Task<List<Receiver>> GetReceiversByEmailsAsync(List<string> emails);
        Task AddReceiversAsync(List<Receiver> receivers);
    }
}
