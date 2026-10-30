using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Filters.Properties;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Common.Filters;

public sealed class WorkspaceFilter(Expression<Func<WorkspaceAggregate, bool>> predicate)
    : Filter<WorkspaceAggregate>(predicate)
{
    public static readonly IReadOnlyList<FilterProperty<WorkspaceAggregate>> FilterProperties =
    [
        new FilterProperties.Workspace.NameFilterProperty(),
        new FilterProperties.Workspace.IsPersonalFilterProperty(),
        new FilterProperties.Workspace.LastActivityProperty(),
        new FilterProperties.Workspace.CreatedDateTimeProperty(),
        new FilterProperties.Workspace.UpdatedDateTimeProperty()
    ];

    public static IReadOnlyList<IFilter<WorkspaceAggregate>> Parse(List<string> search)
    {
        return Parse(search, FilterProperties);
    }
}