using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Json.Serialization.NewtonsoftJson;
using Raven.Client.Documents.Indexes;
using System.Reflection;
using Notifications.Domain.Serializer;

namespace Notifications.Infrastructure
{
    public class RavenDbContext
    {
        private readonly IDocumentStore _store;

        public RavenDbContext(string url, string database)
        {
            _store = new DocumentStore
            {
                Urls = new[] { url },
                Database = database,
                Conventions = new DocumentConventions
                {
                    Serialization = new NewtonsoftJsonSerializationConventions
                    {
                        CustomizeJsonSerializer = serializer =>
                        {
                            serializer.Converters.Add(new EnumToStringConverter());
                        }
                    }
                }
            };
            _store.Initialize();

            IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), _store);

        }

        public IDocumentStore Store => _store;
    }
}