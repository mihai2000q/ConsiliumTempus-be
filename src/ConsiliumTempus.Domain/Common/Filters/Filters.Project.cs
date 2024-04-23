using ConsiliumTempus.Domain.Common.GenericFilters;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Filters;

public static partial class Filters
{
    public static class Project
    {
        public sealed class WorkspaceFilter(WorkspaceId? value)
            : Filter<WorkspaceId?, ProjectAggregate>(value, p => p.Workspace.Id);
        
        public sealed class NameFilter(string? value)
            : StringFilter<ProjectAggregate>(value, p => p.Name.Value);

        public sealed class IsFavoriteFilter(bool? value)
            : BoolFilter<ProjectAggregate>(value, p => p.IsFavorite.Value);
        
        public sealed class IsPrivateFilter(bool? value)
            : BoolFilter<ProjectAggregate>(value, p => p.IsPrivate.Value);
    }
}