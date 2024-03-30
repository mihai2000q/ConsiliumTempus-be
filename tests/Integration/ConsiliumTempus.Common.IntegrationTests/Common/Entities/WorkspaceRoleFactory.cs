using ConsiliumTempus.Domain.Common.Entities;

namespace ConsiliumTempus.Common.IntegrationTests.Common.Entities;

public static class WorkspaceRoleFactory
{
    public static readonly object[] Roles = [WorkspaceRole.Admin, WorkspaceRole.Member, WorkspaceRole.View];
}