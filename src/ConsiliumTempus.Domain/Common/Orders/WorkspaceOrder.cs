using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Common.Orders;

public sealed class WorkspaceOrder : Order<WorkspaceAggregate>
{
    private static class Properties
    {
        public sealed record NameOrderProperty()
            : OrderProperty<WorkspaceAggregate>(nameof(WorkspaceAggregate.Name), p => p.Name.Value);

        public sealed record LastActivityProperty()
            : OrderProperty<WorkspaceAggregate>(nameof(WorkspaceAggregate.LastActivity), p => p.LastActivity);

        public sealed record UpdatedDateTimeProperty()
            : OrderProperty<WorkspaceAggregate>(nameof(WorkspaceAggregate.UpdatedDateTime), p => p.UpdatedDateTime);

        public sealed record CreatedDateTimeProperty()
            : OrderProperty<WorkspaceAggregate>(nameof(WorkspaceAggregate.CreatedDateTime), p => p.CreatedDateTime);
    }
    
    public static readonly IReadOnlyList<OrderProperty<WorkspaceAggregate>> OrderProperties =
    [
        new Properties.NameOrderProperty(),
        new Properties.LastActivityProperty(),
        new Properties.UpdatedDateTimeProperty(),
        new Properties.CreatedDateTimeProperty()
    ];

    private static readonly IReadOnlyDictionary<string, Expression<Func<WorkspaceAggregate, object?>>>
        StringToPropertySelector = ToDictionary(OrderProperties);
    
    private WorkspaceOrder(Expression<Func<WorkspaceAggregate, object?>> propertySelector, OrderType orderType) 
        : base(propertySelector, orderType)
    {
    }
    
    public static WorkspaceOrder? Parse(string? order)
    {
        var orderParsed = Parse(order, StringToPropertySelector);
        return orderParsed is not null
            ? new WorkspaceOrder(orderParsed.PropertySelector, orderParsed.Type)
            : null;
    }
}