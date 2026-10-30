using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Common.Orders.Properties;

internal static partial class OrderProperties
{
    internal static class Workspace
    {
        public sealed record NameOrderProperty() : OrderProperty<WorkspaceAggregate>(
            nameof(WorkspaceAggregate.Name),
            p => p.Name.Value);

        public sealed record LastActivityProperty() : OrderProperty<WorkspaceAggregate>(
            nameof(WorkspaceAggregate.LastActivity),
            p => p.LastActivity);

        public sealed record CreatedDateTimeProperty() : OrderProperty<WorkspaceAggregate>(
            nameof(WorkspaceAggregate.Audit.CreatedDateTime),
            p => p.Audit.CreatedDateTime);

        public sealed record UpdatedDateTimeProperty() : OrderProperty<WorkspaceAggregate>(
            nameof(WorkspaceAggregate.Audit.UpdatedDateTime),
            p => p.Audit.UpdatedDateTime);
    }
}