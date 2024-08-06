using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders.Properties;

namespace ConsiliumTempus.Domain.Common.Orders;

public abstract class MembershipOrder : Order<Membership>
{
    public static readonly IReadOnlyList<OrderProperty<Membership>> OrderProperties =
    [
        new OrderProperties.Membership.UserEmailOrderProperty(),
        new OrderProperties.Membership.UserFirstNameOrderProperty(),
        new OrderProperties.Membership.UserLastNameOrderProperty(),
        new OrderProperties.Membership.UserNameOrderProperty(),
        new OrderProperties.Membership.WorkspaceRoleIdProperty(),
        new OrderProperties.Membership.WorkspaceRoleNameProperty(),
        new OrderProperties.Membership.CreatedDateTimeProperty(),
        new OrderProperties.Membership.UpdatedDateTimeProperty()
    ];

    private MembershipOrder(Expression<Func<Membership, object?>> propertySelector, OrderType orderType)
        : base(propertySelector, orderType)
    {
    }

    public static IReadOnlyList<IOrder<Membership>> Parse(string[]? orders)
    {
        return Parse(orders, OrderProperties);
    }
}