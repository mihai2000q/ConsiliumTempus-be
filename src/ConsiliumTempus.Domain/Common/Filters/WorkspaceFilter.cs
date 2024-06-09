using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Filters.Properties;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Common.Filters;

public abstract class WorkspaceFilter : Filter<WorkspaceAggregate>
{
    public static readonly IReadOnlyList<FilterProperty<WorkspaceAggregate>> FilterProperties =
    [
        new FilterProperties.Workspace.NameFilterProperty(),
        new FilterProperties.Workspace.IsPersonalFilterProperty(),
    ];

    private WorkspaceFilter(Expression<Func<WorkspaceAggregate, bool>> predicate) : base(predicate)
    {
    }

    public static IReadOnlyList<IFilter<WorkspaceAggregate>> Parse(string[]? search)
    {
        return Parse(search, FilterProperties);
    }
}