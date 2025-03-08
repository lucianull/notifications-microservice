using Notifications.Domain.Entities;
using Raven.Client.Documents.Indexes;
using System.Linq;

namespace Notifications.Infrastructure.Indexes
{
    public class Receivers_ByEmailAndPhone : AbstractIndexCreationTask<Receiver>
    {
        public class IndexEntry
        {
            public string CombinedKey { get; set; }
        }

        public Receivers_ByEmailAndPhone()
        {
            Map = receivers => from receiver in receivers
                               select new
                               {
                                   // Compute the combined key as "email:phone"
                                   CombinedKey = receiver.Email + ":" + receiver.Phone
                               };
        }
    }
}
