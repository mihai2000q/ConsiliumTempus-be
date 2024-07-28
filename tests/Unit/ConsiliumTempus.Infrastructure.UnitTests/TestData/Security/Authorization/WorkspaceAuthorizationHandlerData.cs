using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;

public static class WorkspaceAuthorizationHandlerData
{
    internal class GetAuthorizationLevels : TheoryData<WorkspaceAuthorizationLevel>
    {
        public GetAuthorizationLevels()
        {
            Add(WorkspaceAuthorizationLevel.IsCollaborator);
            Add(WorkspaceAuthorizationLevel.IsOwner);
        }
    }
}