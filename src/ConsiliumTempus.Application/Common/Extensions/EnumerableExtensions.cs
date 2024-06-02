using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Application.Common.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<TSource> Paginate<TSource>(
        this IEnumerable<TSource> queryable,
        PaginationInfo? paginationInfo)
    {
        if (paginationInfo is null) return queryable;
        var (pageSize, currentPage) = paginationInfo;
        return queryable
            .Skip(pageSize * (currentPage - 1))
            .Take(pageSize);
    }
}