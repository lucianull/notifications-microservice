using Notifications.Domain.Entities;
using Raven.Client.Documents.Indexes;

namespace Notifications.Infrastructure.Indexes;

public class Receivers_ByEmail : AbstractIndexCreationTask<Receiver>
{
    public Receivers_ByEmail()
    {
        Map = receivers => from receiver in receivers
                           select new
                           {
                               receiver.Email
                           };

        Index(r => r.Email, FieldIndexing.Search);
    }
}
