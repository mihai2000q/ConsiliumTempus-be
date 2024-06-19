using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Domain.Common.Orders.Properties;

internal static partial class OrderProperties
{
    internal static class Project
    {
        internal sealed record NameOrderProperty() : OrderProperty<ProjectAggregate>(
            nameof(ProjectAggregate.Name), 
            p => p.Name.Value);

        internal sealed record LastActivityProperty() : OrderProperty<ProjectAggregate>(
            nameof(ProjectAggregate.LastActivity), 
            p => p.LastActivity);

        internal sealed record CreatedDateTimeProperty() : OrderProperty<ProjectAggregate>(
            nameof(ProjectAggregate.CreatedDateTime), 
            p => p.CreatedDateTime);

        internal sealed record UpdatedDateTimeProperty() : OrderProperty<ProjectAggregate>(
            nameof(ProjectAggregate.UpdatedDateTime),
            p => p.UpdatedDateTime);
    }
}