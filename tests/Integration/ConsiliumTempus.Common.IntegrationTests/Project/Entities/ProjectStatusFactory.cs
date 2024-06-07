using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.Project.Entities;

public static class ProjectStatusFactory
{
    public static ProjectStatus Create(
        Audit audit,
        string title = Constants.ProjectStatus.Title,
        string description = Constants.ProjectStatus.Description,
        ProjectStatusType status = ProjectStatusType.AtRisk)
    {
        var projectStatus = DomainFactory.GetObjectInstance<ProjectStatus>();

        DomainFactory.SetProperty(ref projectStatus, nameof(projectStatus.Id), ProjectStatusId.CreateUnique());
        DomainFactory.SetProperty(ref projectStatus, nameof(projectStatus.Title), Title.Create(title));
        DomainFactory.SetProperty(ref projectStatus, nameof(projectStatus.Status), status);
        DomainFactory.SetProperty(ref projectStatus, nameof(projectStatus.Description), Description.Create(description));
        DomainFactory.SetProperty(ref projectStatus, nameof(projectStatus.Audit), audit);

        return projectStatus;
    }
}