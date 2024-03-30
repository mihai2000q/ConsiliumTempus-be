using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.Project.Entities;

public static class ProjectSprintFactory
{
    public static ProjectSprint Create(
        ProjectAggregate project,
        string name = Constants.ProjectSprint.Name,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null)
    {
        var sprint = DomainFactory.GetObjectInstance<ProjectSprint>();

        DomainFactory.SetProperty(ref sprint, nameof(sprint.Id), ProjectSprintId.CreateUnique());
        DomainFactory.SetProperty(ref sprint, nameof(sprint.Name), Name.Create(name));
        DomainFactory.SetProperty(ref sprint, nameof(sprint.StartDate), startDate);
        DomainFactory.SetProperty(ref sprint, nameof(sprint.EndDate), endDate);
        DomainFactory.SetProperty(ref sprint, nameof(sprint.CreatedDateTime), createdDateTime ?? DateTime.UtcNow);
        DomainFactory.SetProperty(ref sprint, nameof(sprint.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow);
        DomainFactory.SetProperty(ref sprint, nameof(sprint.Project), project);
        
        return sprint;
    }   
}