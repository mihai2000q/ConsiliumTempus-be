using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Domain.Common.Orders;

public sealed class ProjectTaskOrder(
    Expression<Func<ProjectTaskAggregate, object?>> propertySelector, 
    OrderType orderType)
    : Order<ProjectTaskAggregate>(propertySelector, orderType)
{
    public static readonly IReadOnlyList<OrderProperty<ProjectTaskAggregate>> OrderProperties =
    [
        Properties.Name,
        Properties.IsCompleted,
        Properties.CreatedDateTime,
        Properties.UpdatedDateTime
    ];

    public static IReadOnlyList<IOrder<ProjectTaskAggregate>> Parse(List<string> orders)
    {
        return Parse(orders, OrderProperties);
    }

    #region Properties

    private static class Properties
    {
        internal static readonly OrderProperty<ProjectTaskAggregate> Name = new(
            nameof(ProjectTaskAggregate.Name),
            p => p.Name.Value);

        internal static readonly OrderProperty<ProjectTaskAggregate> IsCompleted = new(
            nameof(ProjectTaskAggregate.IsCompleted),
            p => p.IsCompleted.Value);

        internal static readonly OrderProperty<ProjectTaskAggregate> CreatedDateTime = new(
            nameof(ProjectTaskAggregate.Audit.CreatedDateTime),
            p => p.Audit.CreatedDateTime);

        internal static readonly OrderProperty<ProjectTaskAggregate> UpdatedDateTime = new(
            nameof(ProjectTaskAggregate.Audit.UpdatedDateTime),
            p => p.Audit.UpdatedDateTime);
    }

    #endregion
}
