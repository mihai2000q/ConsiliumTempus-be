using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.Common.Models;

public static class Order
{
    public const string Separator = ".";
    public const string Descending = "desc";
    public const string Ascending = "asc";

    public static (string PropertyIdentifier, string OrderType)? Split(string order)
    {
        var splitOrder = order.Trim().Split(Separator);
        if (splitOrder.Length != 2) return null;

        return (splitOrder[0].Trim(), splitOrder[1]);
    }
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
        List<string> orders,
        IReadOnlyList<OrderProperty<TEntity>> orderProperties)
    {
        return orders
            .SelectMany(stringOrder => ParseOrder(stringOrder, orderProperties))
            .ToList();
    }

    private static IEnumerable<Order<TEntity>> ParseOrder(
        string order,
        IReadOnlyList<OrderProperty<TEntity>> orderProperties)
    {
        var (propertyIdentifier, stringOrderType) = Order.Split(order)!.Value;

        var propertySelectors = orderProperties
            .Single(op => op.Identifier == propertyIdentifier)
            .PropertySelectors;
        var orderType = stringOrderType == Order.Descending ? OrderType.Descending : OrderType.Ascending;

        return propertySelectors
            .Select(propertySelector => new Order<TEntity>(propertySelector, orderType));
    }
}