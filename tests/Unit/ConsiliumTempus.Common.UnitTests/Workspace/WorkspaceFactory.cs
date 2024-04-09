using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Common.UnitTests.Workspace;

public static class WorkspaceFactory
{
    public static WorkspaceAggregate Create(
        string name = Constants.Workspace.Name,
        string description = Constants.Workspace.Description,
        UserAggregate? owner = null,
        bool isUserWorkspace = false)
    {
        owner ??= UserFactory.Create();
        var workspace = WorkspaceAggregate.Create(
            Name.Create(name),
            Description.Create(description),
            owner,
            IsUserWorkspace.Create(isUserWorkspace));
        workspace.AddUserMembership(MembershipFactory.Create(owner, workspace));
        return workspace;
    }
    
    public static List<WorkspaceAggregate> CreateList(int count = 5)
    {
        return Enumerable
            .Repeat(0, count)
            .Select(_ => Create())
            .ToList();
    }
}