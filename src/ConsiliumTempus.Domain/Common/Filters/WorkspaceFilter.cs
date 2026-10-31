using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Common.Filters;

public sealed class WorkspaceFilter(Expression<Func<WorkspaceAggregate, bool>> predicate)
    : Filter<WorkspaceAggregate>(predicate)
{
    public static readonly IReadOnlyList<FilterProperty<WorkspaceAggregate>> FilterProperties =
    [
        Properties.Name,
        Properties.IsPersonal,
        Properties.LastActivity,
        Properties.CreatedDateTime,
        Properties.UpdatedDateTime
    ];

    public static IReadOnlyList<IFilter<WorkspaceAggregate>> Parse(List<string> search)
    {
        return Parse(search, FilterProperties);
    }

    #region Properties

    private static class Properties
    {
        internal static readonly FilterProperty<WorkspaceAggregate> Name = new(
            nameof(WorkspaceAggregate.Name),
            w => w.Name.Value);

        internal static readonly FilterProperty<WorkspaceAggregate> IsPersonal = new(
            nameof(WorkspaceAggregate.IsPersonal),
            w => w.IsPersonal.Value);

        internal static readonly FilterProperty<WorkspaceAggregate> LastActivity = new(
            nameof(WorkspaceAggregate.LastActivity),
            w => w.LastActivity.Date);

        internal static readonly FilterProperty<WorkspaceAggregate> CreatedDateTime = new(
            nameof(Audit.CreatedDateTime),
            w => w.Audit.CreatedDateTime.Date);

        internal static readonly FilterProperty<WorkspaceAggregate> UpdatedDateTime = new(
            nameof(Audit.UpdatedDateTime),
            w => w.Audit.UpdatedDateTime.Date);
    }

    #endregion
}
