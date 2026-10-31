using System.Linq.Expressions;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.Entities;

namespace ConsiliumTempus.Infrastructure.Extensions;

internal static class QueryableExtensions
{
    extension<TSource>(IQueryable<TSource> queryable)
    {
        public IQueryable<TSource> ApplyFilters(IEnumerable<IFilter<TSource>> filters)
        {
            return filters.Aggregate(queryable, (query, filter) => query.Where(filter.Predicate));
        }

        public IQueryable<TSource> ApplyOrders<TKey>(
            IReadOnlyList<IOrder<TSource>> orders,
            Expression<Func<TSource, TKey>> tieBreaker)
        {
            if (orders.Count == 0) return queryable;
            var orderedQueryable = queryable.ApplyOrder(orders[0]);
            return orders
                .Skip(1)
                .Aggregate(orderedQueryable, ThenApplyOrder)
                .ThenBy(tieBreaker);
        }

        public IQueryable<TSource> Paginate(PaginationInfo? paginationInfo)
        {
            if (paginationInfo is null) return queryable;
            var (pageSize, currentPage) = paginationInfo;
            return queryable
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize);
        }

        public IQueryable<TSource> WhereIf(bool condition, Expression<Func<TSource, bool>> predicate)
        {
            return condition ? queryable.Where(predicate) : queryable;
        }

        public IQueryable<TSource> OrderByIf<TKey>(bool condition, Expression<Func<TSource, TKey>> keySelector)
        {
            return condition ? queryable.OrderBy(keySelector) : queryable;
        }
        
        private IOrderedQueryable<TSource> ApplyOrder(IOrder<TSource> order)
        {
            return order.Type == OrderType.Descending
                ? queryable.OrderByDescending(order.PropertySelector)
                : queryable.OrderBy(order.PropertySelector);
        }
    }

    extension<TSource>(IOrderedQueryable<TSource> queryable)
    {

        private IOrderedQueryable<TSource> ThenApplyOrder(IOrder<TSource> order)
        {
            return order.Type == OrderType.Descending
                ? queryable.ThenByDescending(order.PropertySelector)
                : queryable.ThenBy(order.PropertySelector);
        }
    }

    extension(IQueryable<CustomField> queryable)
    {
        public IQueryable<CustomField> OfCustomFieldType(CustomFieldSetupAggregate customFieldSetup)
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
    }
}
