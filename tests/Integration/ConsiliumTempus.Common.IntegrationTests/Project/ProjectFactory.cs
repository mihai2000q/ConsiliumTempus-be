using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Common.IntegrationTests.Project;

public static class ProjectFactory
{
    public static ProjectAggregate Create(
        WorkspaceAggregate workspace,
        string name = Constants.Project.Name,
        string description = Constants.Project.Description,
        bool isFavorite = false,
        bool isPrivate = false,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null)
    {
        var project = DomainFactory.GetObjectInstance<ProjectAggregate>();

        DomainFactory.SetProperty(ref project, nameof(project.Id), ProjectId.CreateUnique());
        DomainFactory.SetProperty(ref project, nameof(project.Name), Name.Create(name));
        DomainFactory.SetProperty(ref project, nameof(project.Description), Description.Create(description));
        DomainFactory.SetProperty(ref project, nameof(project.IsFavorite), IsFavorite.Create(isFavorite));
        DomainFactory.SetProperty(ref project, nameof(project.IsPrivate), IsPrivate.Create(isPrivate));
        DomainFactory.SetProperty(ref project, nameof(project.CreatedDateTime), createdDateTime ?? DateTime.UtcNow);
        DomainFactory.SetProperty(ref project, nameof(project.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow);
        DomainFactory.SetProperty(ref project, nameof(project.Workspace), workspace);
        
        return project;
    }
}