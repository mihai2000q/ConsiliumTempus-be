using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Domain.Common.Orders.Properties;

internal static partial class OrderProperties
{
    internal static class ProjectTask
    {
        internal sealed record NameOrderProperty() : OrderProperty<ProjectTaskAggregate>(
            nameof(ProjectTaskAggregate.Name),
            p => p.Name.Value);

        internal sealed record IsCompletedProperty() : OrderProperty<ProjectTaskAggregate>(
            nameof(ProjectTaskAggregate.IsCompleted),
            p => p.IsCompleted.Value);

        internal sealed record CreatedDateTimeProperty() : OrderProperty<ProjectTaskAggregate>(
                nameof(ProjectTaskAggregate.CreatedDateTime), 
                p => p.CreatedDateTime);

        internal sealed record UpdatedDateTimeProperty() : OrderProperty<ProjectTaskAggregate>(
            nameof(ProjectTaskAggregate.UpdatedDateTime), 
            p => p.UpdatedDateTime);
    }
}