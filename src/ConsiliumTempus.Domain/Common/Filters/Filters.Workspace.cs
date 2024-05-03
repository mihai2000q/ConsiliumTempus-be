using ConsiliumTempus.Domain.Common.GenericFilters;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Common.Filters;

public static partial class Filters
{
    public static class Workspace
    {
        public sealed class NameFilter(string? value)
            : StringFilter<WorkspaceAggregate>(value, w => w.Name.Value);
    }
}