using System.Linq.Expressions;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace RavenDBUnitTest.Infrastructure;

public class Repository<TAggregate>
    where TAggregate : Aggregate
{
    private readonly IDocumentStore documentStore;

    public Repository(IDocumentStore documentStore)
    {
        this.documentStore = documentStore;
    }

    public async Task<TAggregate> GetByIdAsync(string id)
    {
        using var session = documentStore.OpenAsyncSession();
        return await session.LoadAsync<TAggregate>(id);
    }

    public async Task<IReadOnlyCollection<TAggregate>> GetAllAsync(
        Expression<Func<TAggregate, bool>> filter = null,
        Expression<Func<TAggregate, object>> orderBy = null,
        bool descending = false)
    {
        using var session = documentStore.OpenSession();

        var query = session.Query<TAggregate>();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = descending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);
        }

        return (await query.ToListAsync());
    }

    public async Task StoreAsync(TAggregate entity)
    {
        using var session = documentStore.OpenAsyncSession();
        await session.StoreAsync(entity);
        await session.SaveChangesAsync();
    }

    public async ValueTask DeleteAsync(string id)
    {
        using var session = documentStore.OpenAsyncSession();
        session.Delete(id);
        await session.SaveChangesAsync();
    }
}