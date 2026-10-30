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
}