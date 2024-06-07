using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.UnitTests.Project.Entities;

public static class ProjectStatusFactory
{
    public static ProjectStatus Create(
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null,
        string title = Constants.ProjectStatus.Title,
        ProjectStatusType status = ProjectStatusType.OnTrack,
        string description = Constants.ProjectStatus.Description)
    {
        return ProjectStatus.Create(
            Title.Create(title),
            status,
            Description.Create(description),
            project ?? ProjectFactory.Create(),
            createdBy ?? UserFactory.Create());
    }
    
    public static List<ProjectStatus> CreateList(int count = 5)
    {
        return Enumerable
            .Range(0, count)
            .Select(_ => Create())
            .ToList();
    }
}