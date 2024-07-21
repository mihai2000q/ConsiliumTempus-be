using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.Entities;

namespace ConsiliumTempus.Common.UnitTests.Workspace.Entities;

public static class WorkspaceInvitationFactory
{
    public static WorkspaceInvitation Create(
        UserAggregate? sender = null,
        UserAggregate? collaborator = null,
        WorkspaceAggregate? workspace = null)
    {
        return WorkspaceInvitation.Create(
            sender ?? UserFactory.Create(),
            collaborator ?? UserFactory.Create(),
            workspace ?? WorkspaceFactory.Create());
    }

    public static List<WorkspaceInvitation> CreateList(
        int stagesCount = 5)
    {
        return Enumerable
            .Range(0, stagesCount)
            .Select(_ => Create())
            .ToList();
    }
}