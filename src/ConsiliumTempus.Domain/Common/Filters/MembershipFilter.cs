using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Filters.Properties;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.Filters;

public abstract class MembershipFilter : Filter<Membership>
{
    public static readonly IReadOnlyList<FilterProperty<Membership>> FilterProperties =
    [
        new FilterProperties.Membership.UserNameFilterProperty(),
        new FilterProperties.Membership.WorkspaceRoleIdFilterProperty(),
    ];

    private MembershipFilter(Expression<Func<Membership, bool>> predicate) : base(predicate)
    {
    }

    public static IReadOnlyList<IFilter<Membership>> Parse(string[]? search)
    {
        return Parse(search, FilterProperties);
    }
}