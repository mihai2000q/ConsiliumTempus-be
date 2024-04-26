using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Domain.Common.OrderProperties;

public static partial class OrderProperties
{
    public static class Project
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
}