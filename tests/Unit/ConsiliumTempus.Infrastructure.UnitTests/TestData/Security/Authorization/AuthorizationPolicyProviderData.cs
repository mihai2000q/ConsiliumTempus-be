using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;

public static class AuthorizationPolicyProviderData
{
    internal class GetWorkspaceAuthorizationLevels : TheoryData<WorkspaceAuthorizationLevel>
    {
        public GetWorkspaceAuthorizationLevels()
        {
            AddRange(Enum.GetValues<WorkspaceAuthorizationLevel>());
        }
    }

    internal class GetProjectAuthorizationLevels : TheoryData<ProjectAuthorizationLevel>
    {
        public GetProjectAuthorizationLevels()
        {
            AddRange(Enum.GetValues<ProjectAuthorizationLevel>());
        }
    }

    internal class GetPermissions : TheoryData<Permissions>
    {
        public GetPermissions()
        {
            AddRange(Enum.GetValues<Permissions>());
        }
    }
}