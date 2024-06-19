using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectSprint;

namespace ConsiliumTempus.Domain.Common.Filters.Properties;

internal static partial class FilterProperties
{
    internal static class ProjectSprint
    {
        internal sealed class NameFilterProperty() : FilterProperty<ProjectSprintAggregate>(
            nameof(ProjectSprintAggregate.Name),
            ps => ps.Name.Value);
    }
}