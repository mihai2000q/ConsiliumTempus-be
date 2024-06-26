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
        
        internal sealed class LastActivityProperty() : FilterProperty<WorkspaceAggregate>(
            nameof(WorkspaceAggregate.LastActivity),
            w => w.LastActivity.Date);
        
        internal sealed class CreatedDateTimeProperty() : FilterProperty<WorkspaceAggregate>(
            nameof(WorkspaceAggregate.CreatedDateTime),
            w => w.CreatedDateTime.Date);
        
        internal sealed class UpdatedDateTimeProperty() : FilterProperty<WorkspaceAggregate>(
            nameof(WorkspaceAggregate.UpdatedDateTime),
            w => w.UpdatedDateTime.Date);
    }
}