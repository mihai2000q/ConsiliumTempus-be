using ConsiliumTempus.Domain.Common.GenericFilters;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Domain.Common.Filters;

public static partial class Filters
{
    public static class Project
    {
        public sealed class NameFilter(string? value)
            : StringFilter<ProjectAggregate>(value, p => p.Name.Value);

        public sealed class IsFavoriteFilter(bool? value)
            : BoolFilter<ProjectAggregate>(value, p => p.IsFavorite.Value);
        
        public sealed class IsPrivateFilter(bool? value)
            : BoolFilter<ProjectAggregate>(value, p => p.IsPrivate.Value);
    }
}