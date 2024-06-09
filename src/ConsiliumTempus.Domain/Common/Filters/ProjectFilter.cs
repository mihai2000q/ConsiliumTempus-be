using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Filters.Properties;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Domain.Common.Filters;

public abstract class ProjectFilter : Filter<ProjectAggregate>
{
    public static readonly IReadOnlyList<FilterProperty<ProjectAggregate>> FilterProperties =
    [
        new FilterProperties.Project.NameFilterProperty(),
        new FilterProperties.Project.IsPrivateProperty(),
        new FilterProperties.Project.LifecycleProperty()
    ];

    private ProjectFilter(Expression<Func<ProjectAggregate, bool>> predicate) : base(predicate)
    {
    }

    public static IReadOnlyList<IFilter<ProjectAggregate>> Parse(string[]? search)
    {
        return Parse(search, FilterProperties);
    }
}