using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Domain.Common.Filters;

public sealed class ProjectFilter(Expression<Func<ProjectAggregate, bool>> predicate)
    : Filter<ProjectAggregate>(predicate)
{
    public static readonly IReadOnlyList<FilterProperty<ProjectAggregate>> FilterProperties =
    [
        Properties.Name,
        Properties.Lifecycle,
        Properties.IsPrivate,
        Properties.LatestStatus,
        Properties.LastActivity,
        Properties.CreatedDateTime,
        Properties.UpdatedDateTime
    ];

    public static IReadOnlyList<IFilter<ProjectAggregate>> Parse(List<string> search)
    {
        return Parse(search, FilterProperties);
    }

    #region Properties

    private static class Properties
    {
        internal static readonly FilterProperty<ProjectAggregate> Name = new(
            nameof(ProjectAggregate.Name),
            p => p.Name.Value);

        internal static readonly FilterProperty<ProjectAggregate> Lifecycle = new(
            nameof(ProjectAggregate.Lifecycle),
            p => p.Lifecycle);

        internal static readonly FilterProperty<ProjectAggregate> IsPrivate = new(
            nameof(ProjectAggregate.IsPrivate),
            p => p.IsPrivate.Value);

        internal static readonly FilterProperty<ProjectAggregate> LatestStatus = new(
            nameof(ProjectAggregate.LatestStatus),
            p => p.Statuses.OrderByDescending(s => s.Audit.CreatedDateTime).First().Status);

        internal static readonly FilterProperty<ProjectAggregate> LastActivity = new(
            nameof(ProjectAggregate.LastActivity),
            p => p.LastActivity.Date);

        internal static readonly FilterProperty<ProjectAggregate> CreatedDateTime = new(
            nameof(Audit.CreatedDateTime),
            p => p.Audit.CreatedDateTime.Date);

        internal static readonly FilterProperty<ProjectAggregate> UpdatedDateTime = new(
            nameof(Audit.UpdatedDateTime),
            p => p.Audit.UpdatedDateTime.Date);
    }

    #endregion
}
