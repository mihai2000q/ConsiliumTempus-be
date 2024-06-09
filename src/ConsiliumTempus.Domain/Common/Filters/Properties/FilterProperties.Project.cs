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
        
        internal sealed class IsPrivateProperty() : FilterProperty<ProjectAggregate>(
            nameof(ProjectAggregate.IsPrivate),
            p => p.IsPrivate.Value);
        
        internal sealed class LifecycleProperty() : FilterProperty<ProjectAggregate>(
            nameof(ProjectAggregate.Lifecycle),
            p => p.Lifecycle);
    }
}