using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Infrastructure.Extensions;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestData.Security.Authorization;

public static class WorkspaceAuthorizationHandlerData
{
    internal class GetAuthorizationLevels : TheoryData<WorkspaceAuthorizationLevel>
    {
        public GetAuthorizationLevels()
        {
            Add(WorkspaceAuthorizationLevel.IsCollaborator);
            Add(WorkspaceAuthorizationLevel.IsWorkspaceOwner);
        }
    }
}