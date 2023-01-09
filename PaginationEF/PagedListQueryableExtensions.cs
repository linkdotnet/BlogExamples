using Microsoft.EntityFrameworkCore;

namespace PaginationEF;

public static class PagedListQueryableExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page, int pageSize, CancellationToken token = default)
    {
        var count = await source.CountAsync(token);
        if (count > 0)
        {
            // Be careful when you have NVARCHAR(MAX) in
            // combination with ToListAsync
            // See here: https://stackoverflow.com/questions/28543293/entity-framework-async-operation-takes-ten-times-as-long-to-complete/28619983
            var items = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return new PagedList<T>(items, count, page, pageSize);
        }

        return new(Enumerable.Empty<T>(), 0, 0, 0);
    }
}