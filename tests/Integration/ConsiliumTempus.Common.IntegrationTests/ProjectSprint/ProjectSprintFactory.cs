using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.IntegrationTests.ProjectSprint;

public static class ProjectSprintFactory
{
    public static ProjectSprintAggregate Create(
        ProjectAggregate project,
        Audit audit,
        string name = Constants.ProjectSprint.Name,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        var sprint = DomainFactory.GetObjectInstance<ProjectSprintAggregate>();
        
        DomainFactory.SetProperty(ref sprint, nameof(sprint.Id), ProjectSprintId.CreateUnique());
        DomainFactory.SetProperty(ref sprint, nameof(sprint.Name), Name.Create(name));
        DomainFactory.SetProperty(ref sprint, nameof(sprint.StartDate), startDate);
        DomainFactory.SetProperty(ref sprint, nameof(sprint.EndDate), endDate);
        DomainFactory.SetProperty(ref sprint, nameof(sprint.Audit), audit);
        DomainFactory.SetProperty(ref sprint, nameof(sprint.Project), project);

        return sprint;
    }
}