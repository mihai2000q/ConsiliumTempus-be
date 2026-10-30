using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders.Properties;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Domain.Common.Orders;

public sealed class ProjectTaskOrder(
    Expression<Func<ProjectTaskAggregate, object?>> propertySelector, 
    OrderType orderType)
    : Order<ProjectTaskAggregate>(propertySelector, orderType)
{
    public static readonly IReadOnlyList<OrderProperty<ProjectTaskAggregate>> OrderProperties =
    [
        new OrderProperties.ProjectTask.NameOrderProperty(),
        new OrderProperties.ProjectTask.IsCompletedProperty(),
        new OrderProperties.ProjectTask.CreatedDateTimeProperty(),
        new OrderProperties.ProjectTask.UpdatedDateTimeProperty()
    ];

    public static IReadOnlyList<IOrder<ProjectTaskAggregate>> Parse(List<string> orders)
    {
        return Parse(orders, OrderProperties);
    }
}