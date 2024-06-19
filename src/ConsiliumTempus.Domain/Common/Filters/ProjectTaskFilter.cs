using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Filters.Properties;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Domain.Common.Filters;

public abstract class ProjectTaskFilter : Filter<ProjectTaskAggregate>
{
    public static readonly IReadOnlyList<FilterProperty<ProjectTaskAggregate>> FilterProperties =
    [
        new FilterProperties.ProjectTask.NameFilterProperty(),
        new FilterProperties.ProjectTask.IsCompletedFilterProperty(),
    ];

    private ProjectTaskFilter(Expression<Func<ProjectTaskAggregate, bool>> predicate) : base(predicate)
    {
    }

    public static IReadOnlyList<IFilter<ProjectTaskAggregate>> Parse(string[]? search)
    {
        return Parse(search, FilterProperties);
    }
}