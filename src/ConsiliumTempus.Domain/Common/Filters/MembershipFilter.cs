using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Filters.Properties;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.Filters;

public sealed class MembershipFilter(Expression<Func<Membership, bool>> predicate)
    : Filter<Membership>(predicate)
{
    public static readonly IReadOnlyList<FilterProperty<Membership>> FilterProperties =
    [
        new FilterProperties.Membership.UserNameFilterProperty(),
        new FilterProperties.Membership.WorkspaceRoleIdFilterProperty(),
        new FilterProperties.Membership.WorkspaceRoleNameFilterProperty()
    ];

    public static IReadOnlyList<IFilter<Membership>> Parse(List<string> search)
    {
        return Parse(search, FilterProperties);
    }
}