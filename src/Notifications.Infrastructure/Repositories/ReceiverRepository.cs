using Notifications.Domain.Entities;
using Raven.Client.Documents.Linq;
using Notifications.Application.Contracts;
using Notifications.Infrastructure.Indexes;
using Notifications.Domain.Models;

namespace Notifications.Infrastructure.Repositories
{
    public class ReceiverRepository : IReceiverRepository
    {
        private readonly RavenDbContext _ravenDbContext;

        public ReceiverRepository(RavenDbContext ravenDbContext)
        {
            _ravenDbContext = ravenDbContext;
        }
        public async Task<List<Receiver>> GetReceiversByEmailsAndPhonesAsync(List<EmailPhonePair> emailPhonePairs)
        {
            using var session = _ravenDbContext.Store.OpenAsyncSession();

            // Pre-compute the combined keys for the given pairs
            var keys = emailPhonePairs
                .Select(pair => pair.Email + ":" + pair.Phone)
                .ToArray();

            // Use the DocumentQuery API to query the index with a WhereIn clause
            var receivers = await session.Advanced
                                         .AsyncDocumentQuery<Receiver, Receivers_ByEmailAndPhone>()
                                         .WhereIn("CombinedKey", keys)
                                         .ToListAsync();

            return receivers;
        }

        public async Task AddReceiversAsync(List<Receiver> receivers)
        {
            if (receivers == null || !receivers.Any())
                return;

            int batchSize = 64; // Control the number of concurrent operations
            var semaphore = new SemaphoreSlim(batchSize);
            var tasks = new List<Task>();

            foreach (var receiver in receivers)
            {
                await semaphore.WaitAsync();
                tasks.Add(Task.Run(async () =>
                {
                    using var session = _ravenDbContext.Store.OpenAsyncSession();
                    await session.StoreAsync(receiver);
                    await session.SaveChangesAsync();
                    semaphore.Release();
                }));
            }

            await Task.WhenAll(tasks);
        }
    }
}
