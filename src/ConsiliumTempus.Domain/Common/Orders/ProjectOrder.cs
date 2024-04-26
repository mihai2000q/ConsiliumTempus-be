using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Domain.Common.Orders;

public sealed class ProjectOrder : Order<ProjectAggregate>
{
    private static class Properties
    {
        public sealed record NameOrderProperty()
            : OrderProperty<ProjectAggregate>(nameof(ProjectAggregate.Name), p => p.Name.Value);

        public sealed record LastActivityProperty()
            : OrderProperty<ProjectAggregate>(nameof(ProjectAggregate.LastActivity), p => p.LastActivity);

        public sealed record UpdatedDateTimeProperty()
            : OrderProperty<ProjectAggregate>(nameof(ProjectAggregate.UpdatedDateTime), p => p.UpdatedDateTime);

        public sealed record CreatedDateTimeProperty()
            : OrderProperty<ProjectAggregate>(nameof(ProjectAggregate.CreatedDateTime), p => p.CreatedDateTime);
    }
    
    public static readonly IReadOnlyList<OrderProperty<ProjectAggregate>> OrderProperties =
    [
        new Properties.NameOrderProperty(),
        new Properties.LastActivityProperty(),
        new Properties.UpdatedDateTimeProperty(),
        new Properties.CreatedDateTimeProperty()
    ];

    private static readonly IReadOnlyDictionary<string, Expression<Func<ProjectAggregate, object?>>>
        StringToPropertySelector = ToDictionary(OrderProperties);
    
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