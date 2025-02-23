using Notifications.Domain.Entities;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Notifications.Application.Contracts;
using Notifications.Infrastructure.Indexes;

namespace Notifications.Infrastructure.Repositories
{
    public class ReceiverRepository : IReceiverRepository
    {
        private readonly RavenDbContext _ravenDbContext;

        public ReceiverRepository(RavenDbContext ravenDbContext)
        {
            _ravenDbContext = ravenDbContext;
        }

        public async Task<List<Receiver>> GetReceiversByEmailsAsync(List<string> emails)
        {
            using var session = _ravenDbContext.Store.OpenAsyncSession();

            return await session
                .Query<Receiver, Receivers_ByEmail>()
                .Where(r => r.Email.In(emails))
                .ToListAsync();
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
