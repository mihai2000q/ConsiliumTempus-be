using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.Project.Entities.Sprint;

public static class ProjectSprintFactory
{
    public static ProjectSprintAggregate Create(
        ProjectAggregate project,
        string name = Constants.ProjectSprint.Name,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null)
    {
        var sprint = DomainFactory.GetObjectInstance<ProjectSprintAggregate>();

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