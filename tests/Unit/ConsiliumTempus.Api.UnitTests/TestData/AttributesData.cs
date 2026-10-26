using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.UnitTests.TestData;

internal static class AttributesData
{
    internal class GetPermissions : TheoryData<Permissions>
    {
        public GetPermissions()
        {
            AddRange(Enum.GetValues<Permissions>());
        }
    }

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
}
