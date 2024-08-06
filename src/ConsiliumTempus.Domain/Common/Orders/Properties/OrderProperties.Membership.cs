using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.Orders.Properties;

internal static partial class OrderProperties
{
    internal static class Membership
    {
        internal sealed record UserEmailOrderProperty() : OrderProperty<Entities.Membership>(
            nameof(Entities.Membership.User) + nameof(Entities.Membership.User.Credentials.Email),
            m => m.User.Credentials.Email);

        internal sealed record UserFirstNameOrderProperty() : OrderProperty<Entities.Membership>(
            nameof(Entities.Membership.User) + nameof(Entities.Membership.User.FirstName),
            m => m.User.FirstName.Value);

        internal sealed record UserLastNameOrderProperty() : OrderProperty<Entities.Membership>(
            nameof(Entities.Membership.User) + nameof(Entities.Membership.User.LastName),
            m => m.User.LastName.Value);

        internal sealed record UserNameOrderProperty() : OrderProperty<Entities.Membership>(
            nameof(Entities.Membership.User) + nameof(Entities.Membership.User.Name),
            m => m.User.FirstName.Value,
            m => m.User.LastName.Value);

        internal sealed record WorkspaceRoleIdProperty() : OrderProperty<Entities.Membership>(
            nameof(Entities.Membership.WorkspaceRole) + nameof(Entities.Membership.WorkspaceRole.Id),
            m => m.WorkspaceRole.Id);

        internal sealed record WorkspaceRoleNameProperty() : OrderProperty<Entities.Membership>(
            nameof(Entities.Membership.WorkspaceRole) + nameof(Entities.Membership.WorkspaceRole.Name),
            m => m.WorkspaceRole.Name);

        internal sealed record CreatedDateTimeProperty() : OrderProperty<Entities.Membership>(
            nameof(Entities.Membership.CreatedDateTime),
            m => m.CreatedDateTime);

        internal sealed record UpdatedDateTimeProperty() : OrderProperty<Entities.Membership>(
            nameof(Entities.Membership.UpdatedDateTime),
            m => m.UpdatedDateTime);
    }
}