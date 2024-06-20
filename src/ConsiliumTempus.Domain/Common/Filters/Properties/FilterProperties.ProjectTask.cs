using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Domain.Common.Filters.Properties;

internal static partial class FilterProperties
{
    internal static class ProjectTask
    {
        internal sealed class NameFilterProperty() : FilterProperty<ProjectTaskAggregate>(
            nameof(ProjectTaskAggregate.Name),
            ps => ps.Name.Value);

        internal sealed class IsCompletedFilterProperty() : FilterProperty<ProjectTaskAggregate>(
            nameof(ProjectTaskAggregate.IsCompleted),
            ps => ps.IsCompleted.Value);
    }
}