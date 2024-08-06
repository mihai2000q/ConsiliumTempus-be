using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.Filters.Properties;

internal static partial class FilterProperties
{
    internal static class Membership
    {
        internal sealed class UserNameFilterProperty() : FilterProperty<Entities.Membership>(
            nameof(Entities.Membership.User) + nameof(Entities.Membership.User.Name),
            m => m.User.FirstName.Value + " " + m.User.LastName.Value);

        internal sealed class WorkspaceRoleIdFilterProperty() : FilterProperty<Entities.Membership>(
            nameof(Entities.Membership.WorkspaceRole) + nameof(Entities.Membership.WorkspaceRole.Name),
            m => m.WorkspaceRole.Name);
    }
}