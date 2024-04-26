using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TSource> ApplyFilters<TSource>(
        this IQueryable<TSource> queryable,
        IEnumerable<IFilter<TSource>> filters
    ) where TSource : notnull
    {
        return filters.Aggregate(queryable, (current, filter) =>
            current.WhereIf(filter.CanBeFiltered, filter.Predicate));
    }

    public static IQueryable<TSource> ApplyOrder<TSource>(
        this IQueryable<TSource> queryable,
        IOrder<TSource>? order)
    {
        if (order is null) return queryable;
        return order.Type == OrderType.Descending
            ? queryable.OrderByDescending(order.PropertySelector)
            : queryable.OrderBy(order.PropertySelector);
    }

    public static IQueryable<TSource> WhereIf<TSource>(
        this IQueryable<TSource> queryable,
        bool condition,
        Expression<Func<TSource, bool>> predicate)
    {
        return condition ? queryable.Where(predicate) : queryable;
    }
}