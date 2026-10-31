using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.Orders;

public sealed class MembershipOrder(
    Expression<Func<Membership, object?>> propertySelector,
    OrderType orderType)
    : Order<Membership>(propertySelector, orderType)
{
    public static readonly IReadOnlyList<OrderProperty<Membership>> OrderProperties =
    [
        Properties.UserEmail,
        Properties.UserFirstName,
        Properties.UserLastName,
        Properties.UserName,
        Properties.WorkspaceRoleId,
        Properties.WorkspaceRoleName,
        Properties.CreatedDateTime,
        Properties.UpdatedDateTime
    ];

    public static IReadOnlyList<IOrder<Membership>> Parse(List<string> orders)
    {
        return Parse(orders, OrderProperties);
    }

    #region Properties

    private static class Properties
    {
        internal static readonly OrderProperty<Membership> UserEmail = new(
            nameof(Membership.User) + nameof(Membership.User.Credentials.Email),
            m => m.User.Credentials.Email);

        internal static readonly OrderProperty<Membership> UserFirstName = new(
            nameof(Membership.User) + nameof(Membership.User.FirstName),
            m => m.User.FirstName.Value);

        internal static readonly OrderProperty<Membership> UserLastName = new(
            nameof(Membership.User) + nameof(Membership.User.LastName),
            m => m.User.LastName.Value);

        internal static readonly OrderProperty<Membership> UserName = new(
            nameof(Membership.User) + nameof(Membership.User.Name),
            m => m.User.FirstName.Value,
            m => m.User.LastName.Value);

        internal static readonly OrderProperty<Membership> WorkspaceRoleId = new(
            nameof(Membership.WorkspaceRole) + nameof(Membership.WorkspaceRole.Id),
            m => m.WorkspaceRole.Id);

        internal static readonly OrderProperty<Membership> WorkspaceRoleName = new(
            nameof(Membership.WorkspaceRole) + nameof(Membership.WorkspaceRole.Name),
            m => m.WorkspaceRole.Name);

        internal static readonly OrderProperty<Membership> CreatedDateTime = new(
            nameof(Membership.Audit.CreatedDateTime),
            m => m.Audit.CreatedDateTime);

        internal static readonly OrderProperty<Membership> UpdatedDateTime = new(
            nameof(Membership.Audit.UpdatedDateTime),
            m => m.Audit.UpdatedDateTime);
    }

    #endregion
}
