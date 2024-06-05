using ConsiliumTempus.Common.UnitTests.Project.Entities;
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
        UserAggregate? owner = null,
        WorkspaceAggregate? workspace = null)
    {
        var project = ProjectAggregate.Create(
            Name.Create(name),
            IsPrivate.Create(isPrivate),
            workspace ?? WorkspaceFactory.Create(),
            owner ?? UserFactory.Create());

        project.ClearDomainEvents();

        return project;
    }

    public static ProjectAggregate CreateWithSprints(
        string name = Constants.Project.Name,
        bool isPrivate = false,
        UserAggregate? owner = null,
        WorkspaceAggregate? workspace = null,
        int sprintsCount = 5,
        DateOnly? sprintEndDate = null)
    {
        var project = ProjectAggregate.Create(
            Name.Create(name),
            IsPrivate.Create(isPrivate),
            workspace ?? WorkspaceFactory.Create(),
            owner ?? UserFactory.Create());

        ProjectSprintFactory
            .CreateList(sprintsCount, project: project, endDate: sprintEndDate)
            .ForEach(project.AddSprint);

        project.ClearDomainEvents();

        return project;
    }
    
    public static ProjectAggregate CreateWithStatuses(
        string name = Constants.Project.Name,
        bool isPrivate = false,
        UserAggregate? owner = null,
        WorkspaceAggregate? workspace = null,
        int statusesCount = 5)
    {
        var project = ProjectAggregate.Create(
            Name.Create(name),
            IsPrivate.Create(isPrivate),
            workspace ?? WorkspaceFactory.Create(),
            owner ?? UserFactory.Create());

        project.ClearDomainEvents();
        
        Enumerable
            .Range(0, statusesCount)
            .ToList()
            .ForEach(_ => project.AddStatus(ProjectStatusFactory.Create(project)));

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