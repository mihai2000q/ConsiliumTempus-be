using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Domain.Common.Orders;

public sealed class ProjectOrder : Order<ProjectAggregate>
{
    private static readonly IEnumerable<OrderProperty<ProjectAggregate>> OrderPropertiesList =
    [
        new OrderProperties.OrderProperties.Project.NameOrderProperty(),
        new OrderProperties.OrderProperties.Project.LastActivityProperty(),
        new OrderProperties.OrderProperties.Project.UpdatedDateTimeProperty(),
        new OrderProperties.OrderProperties.Project.CreatedDateTimeProperty()
    ];

    private static readonly IReadOnlyDictionary<string, Expression<Func<ProjectAggregate, object?>>>
        StringToPropertySelector = ToDictionary(OrderPropertiesList);
    
    private ProjectOrder(Expression<Func<ProjectAggregate, object?>> propertySelector, OrderType orderType)
        : base(propertySelector, orderType)
    {
    }

    public static ProjectOrder? Parse(string? order)
    {
        var orderParsed = Parse(order, StringToPropertySelector);
        return orderParsed is not null
            ? new ProjectOrder(orderParsed.PropertySelector, orderParsed.Type)
            : null;
    }
}