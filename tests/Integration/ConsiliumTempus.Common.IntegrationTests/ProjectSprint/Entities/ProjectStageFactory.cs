using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.ProjectSprint.Entities;

public static class ProjectStageFactory
{
    public static ProjectStage Create(
        ProjectSprintAggregate projectSprint,
        Audit audit,
        string name = Constants.ProjectStage.Name,
        int customOrderPosition = 0)
    {
        return EntityBuilder<ProjectStage>.Empty()
            .WithProperty(nameof(ProjectStage.Id), ProjectStageId.CreateUnique())
            .WithProperty(nameof(ProjectStage.Name), Name.Create(name))
            .WithProperty(nameof(ProjectStage.CustomOrderPosition), CustomOrderPosition.Create(customOrderPosition))
            .WithProperty(nameof(ProjectStage.Sprint), projectSprint)
            .WithProperty(nameof(ProjectStage.Audit), audit)
            .Build();
    }
}