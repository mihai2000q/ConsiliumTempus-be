using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Common.Filters.Properties;

internal static partial class FilterProperties
{
    internal static class Workspace
    {
        internal sealed class NameFilterProperty() : FilterProperty<WorkspaceAggregate>(
            nameof(WorkspaceAggregate.Name),
            w => w.Name.Value);

        internal sealed class IsPersonalFilterProperty() : FilterProperty<WorkspaceAggregate>(
            nameof(WorkspaceAggregate.IsPersonal),
            w => w.IsPersonal.Value);
    }
}