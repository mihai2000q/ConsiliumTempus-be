using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.Common.Models;

public class Order<TEntity> : IOrder<TEntity>
{
    public const string ListSeparator = ",";
    public const string Separator = ".";
    public const string Descending = "desc";
    public const string Ascending = "asc";

    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    private static readonly Dictionary<string, OrderType> StringToOrderType = new()
    {
        { Descending, OrderType.Descending },
        { Ascending, OrderType.Ascending }
    };

    public Expression<Func<TEntity, object?>> PropertySelector { get; }

    public OrderType Type { get; }

    protected Order(Expression<Func<TEntity, object?>> propertySelector, OrderType orderType)
    {
        PropertySelector = propertySelector;
        Type = orderType;
    }

    protected static IReadOnlyList<IOrder<TEntity>> Parse(
        string? orders,
        IReadOnlyDictionary<string, Expression<Func<TEntity, object?>>> stringToPropertySelector)
    {
        if (orders is null) return [];

        return orders.Split(ListSeparator)
            .Select(stringOrder => ParseOrder(stringOrder, stringToPropertySelector))
            .ToList();
    }

    protected static IReadOnlyDictionary<string, Expression<Func<TEntity, object?>>> ToDictionary(
        IEnumerable<OrderProperty<TEntity>> enumerable)
    {
        return enumerable
            .Select(op =>
                new KeyValuePair<string, Expression<Func<TEntity, object?>>>(op.Identifier, op.PropertySelector))
            .ToDictionary();
    }

    private static Order<TEntity> ParseOrder(
        string order,
        IReadOnlyDictionary<string, Expression<Func<TEntity, object?>>> stringToPropertySelector)
    {
        var splitOrder = order.Trim().Split(Separator);

        var propertySelector = stringToPropertySelector[splitOrder[0]];
        var orderType = StringToOrderType[splitOrder[1]];

        return new Order<TEntity>(propertySelector, orderType);
    }
}