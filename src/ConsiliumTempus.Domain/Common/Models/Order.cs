using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.Common.Models;

public class Order<TEntity> : IOrder<TEntity>
{
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

    protected static IOrder<TEntity>? Parse(
        string? order,
        IReadOnlyDictionary<string, Expression<Func<TEntity, object?>>> stringToPropertySelector)
    {
        if (order is null) return null;

        var splitOrder = order.Split(Separator);
        if (!stringToPropertySelector.TryGetValue(splitOrder[0], out var propertySelector)) return null;

        var orderType = StringToOrderType[splitOrder[1]];

        return new Order<TEntity>(propertySelector, orderType);
    }

    protected static IReadOnlyDictionary<string, Expression<Func<TEntity, object?>>> ToDictionary(
        IEnumerable<OrderProperty<TEntity>> enumerable)
    {
        return enumerable
            .Select(op => 
                new KeyValuePair<string, Expression<Func<TEntity, object?>>>(op.Identifier, op.PropertySelector))
            .ToDictionary();
    }
}