using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.Project.Entities.Stage;

public static class ProjectStageFactory
{
    public static ProjectStage Create(
        ProjectSprintAggregate projectSprint,
        string name = Constants.ProjectStage.Name,
        int customOrderPosition = 0)
    {
        var stage = DomainFactory.GetObjectInstance<ProjectStage>();

        DomainFactory.SetProperty(ref stage, nameof(stage.Id), ProjectStageId.CreateUnique());
        DomainFactory.SetProperty(ref stage, nameof(stage.Name), Name.Create(name));
        DomainFactory.SetProperty(ref stage, nameof(stage.CustomOrderPosition), CustomOrderPosition.Create(customOrderPosition));
        DomainFactory.SetProperty(ref stage, nameof(stage.Sprint), projectSprint);

        return stage;
    }
}