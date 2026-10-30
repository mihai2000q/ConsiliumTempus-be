using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Filters.Properties;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectSprint;

namespace ConsiliumTempus.Domain.Common.Filters;

public sealed class ProjectSprintFilter(Expression<Func<ProjectSprintAggregate, bool>> predicate)
    : Filter<ProjectSprintAggregate>(predicate)
{
    public static readonly IReadOnlyList<FilterProperty<ProjectSprintAggregate>> FilterProperties =
    [
        new FilterProperties.ProjectSprint.NameFilterProperty()
    ];

    public static IReadOnlyList<IFilter<ProjectSprintAggregate>> Parse(List<string> search)
    {
        return Parse(search, FilterProperties);
    }

    public static ProjectSprintFilter FromThisYear()
    {
        var date = new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return new ProjectSprintFilter(ps =>
            ps.StartDate >= DateOnly.FromDateTime(date) ||
            ps.EndDate >= DateOnly.FromDateTime(date) ||
            ps.Audit.CreatedDateTime >= date);
    }
}