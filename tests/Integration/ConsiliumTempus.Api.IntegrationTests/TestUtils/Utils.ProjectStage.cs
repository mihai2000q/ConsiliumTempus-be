using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Update;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectStage
    {
        internal static void AssertGetCollectionResponse(
            GetCollectionProjectStageResponse response,
            IReadOnlyList<Domain.ProjectSprint.Entities.ProjectStage> stages)
        {
            response.Stages
                .Zip(stages.OrderBy(s => s.CustomOrderPosition.Value))
                .Should().AllSatisfy(x => AssertResponse(x.First, x.Second));
        }
        
        internal static void AssertCreation(
            Domain.ProjectSprint.Entities.ProjectStage stage,
            CreateProjectStageRequest request)
        {
            stage.Name.Value.Should().Be(request.Name);
            stage.CustomOrderPosition.Value.Should().Be(stage.Sprint.Stages.Count - 1);
            stage.Sprint.Id.Value.Should().Be(request.ProjectSprintId);

            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }

        internal static void AssertUpdated(
            Domain.ProjectSprint.Entities.ProjectStage stage,
            Domain.ProjectSprint.Entities.ProjectStage newStage,
            UpdateProjectStageRequest request)
        {
            // unchanged
            newStage.Id.Value.Should().Be(request.Id);
            newStage.Sprint.Should().Be(stage.Sprint);

            // changed
            newStage.Name.Value.Should().Be(request.Name);

            newStage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            newStage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }

        private static void AssertResponse(
            GetCollectionProjectStageResponse.ProjectStageResponse response,
            Domain.ProjectSprint.Entities.ProjectStage projectStage)
        {
            response.Id.Should().Be(projectStage.Id.Value);
            response.Name.Should().Be(projectStage.Name.Value);
        }
    }
}