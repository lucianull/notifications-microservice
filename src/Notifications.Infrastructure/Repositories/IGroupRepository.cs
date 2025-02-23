using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Repositories;

public interface IGroupRepository
{
    Task<Group?> GetGroupByReceiverIdsAsync(List<string> receiverIds);

    Task AddGroupAsync(Group group);

}
