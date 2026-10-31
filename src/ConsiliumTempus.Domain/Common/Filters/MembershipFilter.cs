using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.Filters;

public sealed class MembershipFilter(Expression<Func<Membership, bool>> predicate)
    : Filter<Membership>(predicate)
{
    public static readonly IReadOnlyList<FilterProperty<Membership>> FilterProperties =
    [
        Properties.UserName,
        Properties.WorkspaceRoleId,
        Properties.WorkspaceRoleName
    ];

    public static IReadOnlyList<IFilter<Membership>> Parse(List<string> search)
    {
        return Parse(search, FilterProperties);
    }

    #region Properties

    private static class Properties
    {
        internal static readonly FilterProperty<Membership> UserName = new(
            nameof(Membership.User) + nameof(Membership.User.Name),
            m => m.User.FirstName.Value + " " + m.User.LastName.Value);

        internal static readonly FilterProperty<Membership> WorkspaceRoleId = new(
            nameof(Membership.WorkspaceRole) + nameof(Membership.WorkspaceRole.Id),
            m => m.WorkspaceRole.Id);

        internal static readonly FilterProperty<Membership> WorkspaceRoleName = new(
            nameof(Membership.WorkspaceRole) + nameof(Membership.WorkspaceRole.Name),
            m => m.WorkspaceRole.Name);
    }

    #endregion
}
