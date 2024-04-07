using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectFactory
{
    public static ProjectAggregate Create(
        string name = Constants.Project.Name,
        string description = Constants.Project.Description,
        bool isPrivate = false,
        UserAggregate? user = null,
        WorkspaceAggregate? workspace = null)
    {
        return ProjectAggregate.Create(
            Name.Create(name),
            Description.Create(description),
            IsPrivate.Create(isPrivate),
            workspace ?? WorkspaceFactory.Create(),
            user ?? UserFactory.Create());
    }
    
    public static List<ProjectAggregate> CreateList(int count = 5)
    {
        return Enumerable
            .Repeat(0, count)
            .Select(_ => Create())
            .ToList();
    }
}