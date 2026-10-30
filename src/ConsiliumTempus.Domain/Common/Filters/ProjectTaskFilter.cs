using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Filters.Properties;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Domain.Common.Filters;

public sealed class ProjectTaskFilter(Expression<Func<ProjectTaskAggregate, bool>> predicate)
    : Filter<ProjectTaskAggregate>(predicate)
{
    public static readonly IReadOnlyList<FilterProperty<ProjectTaskAggregate>> FilterProperties =
    [
        new FilterProperties.ProjectTask.NameFilterProperty(),
        new FilterProperties.ProjectTask.IsCompletedFilterProperty()
    ];

    public static IReadOnlyList<IFilter<ProjectTaskAggregate>> Parse(List<string> search)
    {
        return Parse(search, FilterProperties);
    }
}