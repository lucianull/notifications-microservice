using Notifications.Domain.Entities;
using Notifications.Domain.Models;

namespace Notifications.Application.Contracts
{
    public interface IReceiverRepository
    {
        Task<List<Receiver>> GetReceiversByEmailsAndPhonesAsync(List<EmailPhonePair> emailPhonePairs);
        Task AddReceiversAsync(List<Receiver> receivers);

    }
}
