using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Domain.Common.Orders;

public sealed class ProjectOrder(
    Expression<Func<ProjectAggregate, object?>> propertySelector, 
    OrderType orderType)
    : Order<ProjectAggregate>(propertySelector, orderType)
{
    public static readonly IReadOnlyList<OrderProperty<ProjectAggregate>> OrderProperties =
    [
        Properties.Name,
        Properties.LastActivity,
        Properties.CreatedDateTime,
        Properties.UpdatedDateTime
    ];

    public static IReadOnlyList<IOrder<ProjectAggregate>> Parse(List<string> orders)
    {
        return Parse(orders, OrderProperties);
    }

    #region Properties

    private static class Properties
    {
        internal static readonly OrderProperty<ProjectAggregate> Name = new(
            nameof(ProjectAggregate.Name),
            p => p.Name.Value);

        internal static readonly OrderProperty<ProjectAggregate> LastActivity = new(
            nameof(ProjectAggregate.LastActivity),
            p => p.LastActivity);

        internal static readonly OrderProperty<ProjectAggregate> CreatedDateTime = new(
            nameof(ProjectAggregate.Audit.CreatedDateTime),
            p => p.Audit.CreatedDateTime);

        internal static readonly OrderProperty<ProjectAggregate> UpdatedDateTime = new(
            nameof(ProjectAggregate.Audit.UpdatedDateTime),
            p => p.Audit.UpdatedDateTime);
    }

    #endregion
}
