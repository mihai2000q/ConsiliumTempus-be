using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Domain.Common.Filters.Properties;

internal static partial class FilterProperties
{
    internal static class Project
    {
        internal sealed class NameFilterProperty() : FilterProperty<ProjectAggregate>(
            nameof(ProjectAggregate.Name),
            p => p.Name.Value);

        internal sealed class LifecycleProperty() : FilterProperty<ProjectAggregate>(
            nameof(ProjectAggregate.Lifecycle),
            p => p.Lifecycle);

        internal sealed class IsPrivateProperty() : FilterProperty<ProjectAggregate>(
            nameof(ProjectAggregate.IsPrivate),
            p => p.IsPrivate.Value);
        
        internal sealed class LatestStatusProperty() : FilterProperty<ProjectAggregate>(
            nameof(ProjectAggregate.LatestStatus),
            p => p.Statuses.OrderByDescending(s => s.Audit.CreatedDateTime).First().Status);
        
        internal sealed class LastActivityProperty() : FilterProperty<ProjectAggregate>(
            nameof(ProjectAggregate.LastActivity),
            p => p.LastActivity.Date);
        
        internal sealed class CreatedDateTimeProperty() : FilterProperty<ProjectAggregate>(
            nameof(ProjectAggregate.CreatedDateTime),
            p => p.CreatedDateTime.Date);
        
        internal sealed class UpdatedDateTimeProperty() : FilterProperty<ProjectAggregate>(
            nameof(ProjectAggregate.UpdatedDateTime),
            p => p.UpdatedDateTime.Date);
    }
}