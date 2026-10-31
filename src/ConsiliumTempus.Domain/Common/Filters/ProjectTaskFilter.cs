using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Domain.Common.Filters;

public sealed class ProjectTaskFilter(Expression<Func<ProjectTaskAggregate, bool>> predicate)
    : Filter<ProjectTaskAggregate>(predicate)
{
    public static readonly IReadOnlyList<FilterProperty<ProjectTaskAggregate>> FilterProperties =
    [
        Properties.Name,
        Properties.IsCompleted
    ];

    public static IReadOnlyList<IFilter<ProjectTaskAggregate>> Parse(List<string> search)
    {
        return Parse(search, FilterProperties);
    }

    #region Properties

    private static class Properties
    {
        internal static readonly FilterProperty<ProjectTaskAggregate> Name = new(
            nameof(ProjectTaskAggregate.Name),
            ps => ps.Name.Value);

        internal static readonly FilterProperty<ProjectTaskAggregate> IsCompleted = new(
            nameof(ProjectTaskAggregate.IsCompleted),
            ps => ps.IsCompleted.Value);
    }

    #endregion
}
