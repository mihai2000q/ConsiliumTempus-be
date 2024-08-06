using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.Common.Models;

public static class Order
{
    public const string Separator = ".";
    public const string Descending = "desc";
    public const string Ascending = "asc";
}

public class Order<TEntity> : IOrder<TEntity>
{
    public Expression<Func<TEntity, object?>> PropertySelector { get; }

    public OrderType Type { get; }

    protected Order(Expression<Func<TEntity, object?>> propertySelector, OrderType orderType)
    {
        PropertySelector = propertySelector;
        Type = orderType;
    }

    protected static IReadOnlyList<IOrder<TEntity>> Parse(
        string[]? orders,
        IReadOnlyList<OrderProperty<TEntity>> orderProperties)
    {
        if (orders is null) return [];

        return orders
            .SelectMany(stringOrder => ParseOrder(stringOrder, orderProperties))
            .ToList();
    }

    private static IEnumerable<Order<TEntity>> ParseOrder(
        string order,
        IReadOnlyList<OrderProperty<TEntity>> orderProperties)
    {
        var splitOrder = order.Trim().Split(Order.Separator);

        var propertySelectors = orderProperties
            .Single(op => op.Identifier == splitOrder[0])
            .PropertySelectors;
        var orderType = splitOrder[1] == Order.Descending ? OrderType.Descending : OrderType.Ascending;

        return propertySelectors
            .Select(propertySelector => new Order<TEntity>(propertySelector, orderType));
    }
}