using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TSource> ApplyFilters<TSource>(
        this IQueryable<TSource> queryable,
        IEnumerable<IFilter<TSource>> filters) 
    {
        return filters.Aggregate(queryable, (query, filter) => query.Where(filter.Predicate));
    }

    public static IQueryable<TSource> ApplyOrders<TSource>(
        this IQueryable<TSource> queryable,
        IReadOnlyList<IOrder<TSource>> orders)
    {
        if (orders.Count == 0) return queryable;
        var orderedQueryable = queryable.ApplyOrder(orders[0]);
        for (var i = 1; i < orders.Count; i++)
        {
            orderedQueryable = orderedQueryable.ThenApplyOrder(orders[i]);
        }

        return orderedQueryable;
    }

    public static IQueryable<TSource> Paginate<TSource>(
        this IQueryable<TSource> queryable,
        PaginationInfo? paginationInfo)
    {
        if (paginationInfo is null) return queryable;
        var (pageSize, currentPage) = paginationInfo;
        return queryable
            .Skip(pageSize * (currentPage - 1))
            .Take(pageSize);
    }

    public static IQueryable<TSource> WhereIf<TSource>(
        this IQueryable<TSource> queryable,
        bool condition,
        Expression<Func<TSource, bool>> predicate)
    {
        return condition ? queryable.Where(predicate) : queryable;
    }

    public static IQueryable<TSource> OrderByIf<TSource, TKey>(
        this IQueryable<TSource> queryable,
        bool condition,
        Expression<Func<TSource, TKey>> keySelector)
    {
        return condition ? queryable.OrderBy(keySelector) : queryable;
    }

    private static IOrderedQueryable<TSource> ApplyOrder<TSource>(
        this IQueryable<TSource> queryable,
        IOrder<TSource> order)
    {
        return order.Type == OrderType.Descending
            ? queryable.OrderByDescending(order.PropertySelector)
            : queryable.OrderBy(order.PropertySelector);
    }

    private static IOrderedQueryable<TSource> ThenApplyOrder<TSource>(
        this IOrderedQueryable<TSource> queryable,
        IOrder<TSource> order)
    {
        return order.Type == OrderType.Descending
            ? queryable.ThenByDescending(order.PropertySelector)
            : queryable.ThenBy(order.PropertySelector);
    }
}