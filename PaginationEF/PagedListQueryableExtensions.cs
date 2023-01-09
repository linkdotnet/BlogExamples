using Microsoft.EntityFrameworkCore;

namespace PaginationEF;

public static class PagedListQueryableExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page, int pageSize, CancellationToken token = default)
    {
        var count = await source.CountAsync(token);
        if (count > 0)
        {
            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(token);
            return new PagedList<T>(items, count, page, pageSize);
        }

        return new(Enumerable.Empty<T>(), 0, 0, 0);
    }
}