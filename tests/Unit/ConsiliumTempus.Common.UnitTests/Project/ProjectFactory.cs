using ConsiliumTempus.Common.UnitTests.ProjectSprint;
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
        bool isPrivate = false,
        UserAggregate? user = null,
        WorkspaceAggregate? workspace = null)
    {
        var project = ProjectAggregate.Create(
            Name.Create(name),
            IsPrivate.Create(isPrivate),
            workspace ?? WorkspaceFactory.Create(),
            user ?? UserFactory.Create());

        project.ClearDomainEvents();

        return project;
    }

    public static ProjectAggregate CreateWithSprints(
        string name = Constants.Project.Name,
        bool isPrivate = false,
        UserAggregate? user = null,
        WorkspaceAggregate? workspace = null,
        int sprintsCount = 5)
    {
        var project = ProjectAggregate.Create(
            Name.Create(name),
            IsPrivate.Create(isPrivate),
            workspace ?? WorkspaceFactory.Create(),
            user ?? UserFactory.Create());

        ProjectSprintFactory
            .CreateList(sprintsCount, project: project)
            .ForEach(project.AddSprint);

        project.ClearDomainEvents();

        return project;
    }

    public static List<ProjectAggregate> CreateList(int count = 5)
    {
        return Enumerable
            .Range(0, count)
            .Select(_ => CreateWithSprints())
            .ToList();
    }
}