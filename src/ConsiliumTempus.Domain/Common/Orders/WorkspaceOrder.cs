using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders.Properties;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Common.Orders;

public sealed class WorkspaceOrder(
    Expression<Func<WorkspaceAggregate, object?>> propertySelector, 
    OrderType orderType)
    : Order<WorkspaceAggregate>(propertySelector, orderType)
{
    public static readonly IReadOnlyList<OrderProperty<WorkspaceAggregate>> OrderProperties =
    [
        new OrderProperties.Workspace.NameOrderProperty(),
        new OrderProperties.Workspace.LastActivityProperty(),
        new OrderProperties.Workspace.UpdatedDateTimeProperty(),
        new OrderProperties.Workspace.CreatedDateTimeProperty()
    ];

    public static IReadOnlyList<IOrder<WorkspaceAggregate>> Parse(List<string> orders)
    {
        return Parse(orders, OrderProperties);
    }

    // The user can also be a member of other people's personal workspaces,
    // so only the one that the user owns is the personal workspace of the user
    public static WorkspaceOrder PersonalFirst(UserAggregate user)
    {
        return new WorkspaceOrder(w => w.Owner == user && w.IsPersonal.Value, OrderType.Descending);
    }
}