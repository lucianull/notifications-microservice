using Notifications.Domain.Entities;
using Notifications.Infrastructure.Indexes;
using Raven.Client.Documents;

namespace Notifications.Infrastructure.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly RavenDbContext _ravenDbContext;

        public GroupRepository(RavenDbContext ravenDbContext)
        {
            _ravenDbContext = ravenDbContext;
        }

        public async Task<Group?> GetGroupByReceiverIdsAsync(List<string> receiverIds)
        {
            using var session = _ravenDbContext.Store.OpenAsyncSession();

            var key = string.Join(",", receiverIds.OrderBy(id => id));

            // Fetch the correct ID
            var indexEntry = await session.Query<Groups_ByReceiverIdsKey.IndexEntry, Groups_ByReceiverIdsKey>()
                                          .Where(x => x.ReceiverIdsKey == key)
                                          .SingleOrDefaultAsync();

            if (indexEntry == null || string.IsNullOrEmpty(indexEntry.GroupId))
                return null;

            // Load the Group entity by ID
            return await session.LoadAsync<Group>(indexEntry.GroupId);
        }

        public async Task AddGroupAsync(Group group)
        {
            using var session = _ravenDbContext.Store.OpenAsyncSession();
            await session.StoreAsync(group);
            await session.SaveChangesAsync();
        }
    }
}
