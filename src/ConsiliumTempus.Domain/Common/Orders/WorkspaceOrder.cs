using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders.Properties;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Common.Orders;

public abstract class WorkspaceOrder : Order<WorkspaceAggregate>
{
    public static readonly IReadOnlyList<OrderProperty<WorkspaceAggregate>> OrderProperties =
    [
        new OrderProperties.Workspace.NameOrderProperty(),
        new OrderProperties.Workspace.LastActivityProperty(),
        new OrderProperties.Workspace.UpdatedDateTimeProperty(),
        new OrderProperties.Workspace.CreatedDateTimeProperty()
    ];

    private static readonly IReadOnlyDictionary<string, Expression<Func<WorkspaceAggregate, object?>>>
        StringToPropertySelector = ToDictionary(OrderProperties);
    
    private WorkspaceOrder(Expression<Func<WorkspaceAggregate, object?>> propertySelector, OrderType orderType) 
        : base(propertySelector, orderType)
    {
    }
    
    public static IReadOnlyList<IOrder<WorkspaceAggregate>> Parse(string? orders)
    {
        return Parse(orders, StringToPropertySelector);
    }
}