using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;

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
        return EntityBuilder<ProjectSprintAggregate>.Empty()
            .WithProperty(nameof(ProjectSprintAggregate.Id), ProjectSprintId.CreateUnique())
            .WithProperty(nameof(ProjectSprintAggregate.Name), Name.Create(name))
            .WithProperty(nameof(ProjectSprintAggregate.StartDate), startDate)
            .WithProperty(nameof(ProjectSprintAggregate.EndDate), endDate)
            .WithProperty(nameof(ProjectSprintAggregate.Audit), audit)
            .WithProperty(nameof(ProjectSprintAggregate.Project), project)
            .Build();
    }
}