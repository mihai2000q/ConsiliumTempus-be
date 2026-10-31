using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
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
        Properties.Name,
        Properties.LastActivity,
        Properties.UpdatedDateTime,
        Properties.CreatedDateTime
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

    #region Properties

    private static class Properties
    {
        internal static readonly OrderProperty<WorkspaceAggregate> Name = new(
            nameof(WorkspaceAggregate.Name),
            p => p.Name.Value);

        internal static readonly OrderProperty<WorkspaceAggregate> LastActivity = new(
            nameof(WorkspaceAggregate.LastActivity),
            p => p.LastActivity);

        internal static readonly OrderProperty<WorkspaceAggregate> UpdatedDateTime = new(
            nameof(WorkspaceAggregate.Audit.UpdatedDateTime),
            p => p.Audit.UpdatedDateTime);

        internal static readonly OrderProperty<WorkspaceAggregate> CreatedDateTime = new(
            nameof(WorkspaceAggregate.Audit.CreatedDateTime),
            p => p.Audit.CreatedDateTime);
    }

    #endregion
}
