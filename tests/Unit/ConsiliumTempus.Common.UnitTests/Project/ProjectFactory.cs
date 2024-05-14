using ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;
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
        WorkspaceAggregate? workspace = null,
        int sprintsCount = 5)
    {
        var project = ProjectAggregate.Create(
            Name.Create(name),
            Description.Create(description),
            IsPrivate.Create(isPrivate),
            workspace ?? WorkspaceFactory.Create(),
            user ?? UserFactory.Create());

        ProjectSprintFactory
            .CreateList(sprintsCount, project: project)
            .ForEach(s => project.AddSprint(s));

        return project;
    }

    public static List<ProjectAggregate> CreateList(int count = 5)
    {
        return Enumerable
            .Repeat(0, count)
            .Select(_ => Create())
            .ToList();
    }
}