using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Filters.Properties;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectSprint;

namespace ConsiliumTempus.Domain.Common.Filters;

public abstract class ProjectSprintFilter : Filter<ProjectSprintAggregate>
{
    public static readonly IReadOnlyList<FilterProperty<ProjectSprintAggregate>> FilterProperties =
    [
        new FilterProperties.ProjectSprint.NameFilterProperty(),
    ];

    private ProjectSprintFilter(Expression<Func<ProjectSprintAggregate, bool>> predicate) : base(predicate)
    {
    }

    public static IReadOnlyList<IFilter<ProjectSprintAggregate>> Parse(string[]? search)
    {
        return Parse(search, FilterProperties);
    }
}