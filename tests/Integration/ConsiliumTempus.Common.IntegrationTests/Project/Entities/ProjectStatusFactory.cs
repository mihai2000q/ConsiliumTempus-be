using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.Project.Entities;

public static class ProjectStatusFactory
{
    public static ProjectStatus Create(
        ProjectAggregate project,
        Audit audit,
        string title = Constants.ProjectStatus.Title,
        string description = Constants.ProjectStatus.Description,
        ProjectStatusType status = ProjectStatusType.AtRisk)
    {
        return EntityBuilder<ProjectStatus>.Empty()
            .WithProperty(nameof(ProjectStatus.Id), ProjectStatusId.CreateUnique())
            .WithProperty(nameof(ProjectStatus.Title), Title.Create(title))
            .WithProperty(nameof(ProjectStatus.Status), status)
            .WithProperty(nameof(ProjectStatus.Description), Description.Create(description))
            .WithProperty(nameof(ProjectStatus.Project), project)
            .WithProperty(nameof(ProjectStatus.Audit), audit)
            .Build();
    }
}