using System.Collections.Concurrent;
using Raven.Client.Documents;

public interface IDocumentStoreFactory
{
    IDocumentStore GetStore(string tenantId);
}

public class DocumentStoreFactory : IDocumentStoreFactory
{
    private readonly ConcurrentDictionary<string, IDocumentStore> _stores;

    public DocumentStoreFactory()
    {
        _stores = new ConcurrentDictionary<string, IDocumentStore>();
    }

    public IDocumentStore GetStore(string tenantId)
    {
        if (_stores.TryGetValue(tenantId, out IDocumentStore value))
        {
            return value;
        }

        var store = new DocumentStore
        {
            Urls = new[] { "http://localhost:8080" },
            Database = tenantId
        };

        store.Initialize();

        _stores[tenantId] = store;

        return store;
    }
}