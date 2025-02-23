using Notifications.Domain.Entities;
using Raven.Client.Documents.Indexes;
using System.Linq;

namespace Notifications.Infrastructure.Indexes;

public class Groups_ByReceiverIdsKey : AbstractIndexCreationTask<Group, Groups_ByReceiverIdsKey.IndexEntry>
{
    public class IndexEntry
    {
        public string GroupId { get; set; }
        public string ReceiverIdsKey { get; set; }
    }

    public Groups_ByReceiverIdsKey()
    {
        Map = groups => from g in groups
                        select new IndexEntry
                        {
                            GroupId = g.Id,
                            ReceiverIdsKey = string.Join(",", g.ReceiverIds.OrderBy(id => id))
                        };

        Reduce = results => from result in results
                            group result by result.ReceiverIdsKey into g
                            select new IndexEntry
                            {
                                GroupId = g.Select(x => x.GroupId).FirstOrDefault(),
                                ReceiverIdsKey = g.Key
                            };

        Index(x => x.ReceiverIdsKey, FieldIndexing.Exact);
    }
}
