using Raven.Client.Documents;

public interface ITenantDocumentStore
{
    IDocumentStore DocumentStore { get; }
}

public class TenantDocumentStore : ITenantDocumentStore
{
    public TenantDocumentStore(string tenantId, IDocumentStoreFactory factory)
    {
        DocumentStore = factory.GetStore(tenantId);
    }

    public IDocumentStore DocumentStore { get; }
}