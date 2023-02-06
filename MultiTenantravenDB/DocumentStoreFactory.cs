using System.Collections.Concurrent;
using Raven.Client.Documents;

public interface IDocumentStoreFactory
{
    IDocumentStore GetStore(string tenantId);
}

public class DocumentStoreFactory : IDocumentStoreFactory
{
    private readonly ConcurrentDictionary<string, Lazy<IDocumentStore>> _stores;

    public DocumentStoreFactory()
    {
        _stores = new ConcurrentDictionary<string, Lazy<IDocumentStore>>();
    }

    public IDocumentStore GetStore(string tenantId)
    {
        if (_stores.TryGetValue(tenantId, out var value))
        {
            return value.Value;
        }

        var store = new DocumentStore
        {
            Urls = new[] { "http://localhost:8080" },
            Database = tenantId
        };

        store.Initialize();

        _stores[tenantId] = new Lazy<IDocumentStore>(store);

        return store;
    }
}