using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders.Properties;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Domain.Common.Orders;

public abstract class ProjectOrder : Order<ProjectAggregate>
{
    public static readonly IReadOnlyList<OrderProperty<ProjectAggregate>> OrderProperties =
    [
        new OrderProperties.Project.NameOrderProperty(),
        new OrderProperties.Project.LastActivityProperty(),
        new OrderProperties.Project.CreatedDateTimeProperty(),
        new OrderProperties.Project.UpdatedDateTimeProperty()
    ];

    private static readonly IReadOnlyDictionary<string, Expression<Func<ProjectAggregate, object?>>>
        StringToPropertySelector = ToDictionary(OrderProperties);
    
    private ProjectOrder(Expression<Func<ProjectAggregate, object?>> propertySelector, OrderType orderType)
        : base(propertySelector, orderType)
    {
    }

    public static IReadOnlyList<IOrder<ProjectAggregate>> Parse(string[]? orders)
    {
        return Parse(orders, StringToPropertySelector);
    }
}