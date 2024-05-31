using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.Common.Models;

public class Order<TEntity> : IOrder<TEntity>
{
    public const string Separator = ".";
    public const string Descending = "desc";
    public const string Ascending = "asc";

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
            .Select(stringOrder => ParseOrder(stringOrder, orderProperties))
            .ToList();
    }

    private static Order<TEntity> ParseOrder(
        string order,
        IReadOnlyList<OrderProperty<TEntity>> orderProperties)
    {
        var splitOrder = order.Trim().Split(Separator);

        var propertySelector = orderProperties
            .Single(op => op.Identifier == splitOrder[0])
            .PropertySelector;
        var orderType = splitOrder[1] == Descending ? OrderType.Descending : OrderType.Ascending;

        return new Order<TEntity>(propertySelector, orderType);
    }
}