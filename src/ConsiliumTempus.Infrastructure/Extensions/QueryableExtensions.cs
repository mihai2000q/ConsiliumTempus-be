using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.Entities;

namespace ConsiliumTempus.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<CustomField> OfCustomFieldType(
        this IQueryable<CustomField> queryable,
        CustomFieldSetupAggregate customFieldSetup)
    {
        return customFieldSetup switch
        {
            DateCustomFieldSetupAggregate => queryable.OfType<DateCustomField>(),
            DateTimeCustomFieldSetupAggregate => queryable.OfType<DateTimeCustomField>(),
            DurationCustomFieldSetupAggregate => queryable.OfType<DurationCustomField>(),
            MultiSelectCustomFieldSetupAggregate => queryable.OfType<MultiSelectCustomField>(),
            NumberCustomFieldSetupAggregate => queryable.OfType<NumberCustomField>(),
            PeopleCustomFieldSetupAggregate => queryable.OfType<PeopleCustomField>(),
            SingleSelectCustomFieldSetupAggregate => queryable.OfType<SingleSelectCustomField>(),
            TextCustomFieldSetupAggregate => queryable.OfType<TextCustomField>(),
            TimeCustomFieldSetupAggregate => queryable.OfType<TimeCustomField>(),
            _ => throw new ArgumentOutOfRangeException(nameof(customFieldSetup), customFieldSetup, "Type Not Supported")
        };
    }

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
        return orders
            .Skip(1)
            .Aggregate(orderedQueryable, ThenApplyOrder);
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