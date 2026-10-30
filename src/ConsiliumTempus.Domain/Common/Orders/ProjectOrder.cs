using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders.Properties;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Domain.Common.Orders;

public sealed class ProjectOrder(
    Expression<Func<ProjectAggregate, object?>> propertySelector, 
    OrderType orderType)
    : Order<ProjectAggregate>(propertySelector, orderType)
{
    public static readonly IReadOnlyList<OrderProperty<ProjectAggregate>> OrderProperties =
    [
        new OrderProperties.Project.NameOrderProperty(),
        new OrderProperties.Project.LastActivityProperty(),
        new OrderProperties.Project.CreatedDateTimeProperty(),
        new OrderProperties.Project.UpdatedDateTimeProperty()
    ];

    public static IReadOnlyList<IOrder<ProjectAggregate>> Parse(List<string> orders)
    {
        return Parse(orders, OrderProperties);
    }
}