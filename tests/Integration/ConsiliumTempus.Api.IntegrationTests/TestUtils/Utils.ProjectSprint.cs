using ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Create;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Get;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectSprint.RemoveStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Update;
using ConsiliumTempus.Api.Contracts.ProjectSprint.UpdateStage;
using ConsiliumTempus.Domain.ProjectSprint;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectSprint
    {
        internal static void AssertGetResponse(
            GetProjectSprintResponse response,
            ProjectSprintAggregate sprint)
        {
            response.Name.Should().Be(sprint.Name.Value);
            response.StartDate.Should().Be(sprint.StartDate);
            response.EndDate.Should().Be(sprint.EndDate);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionProjectSprintResponse response,
            IReadOnlyList<ProjectSprintAggregate> sprints)
        {
            response.Sprints.Should().HaveCount(sprints.Count);
            response.Sprints
                .Zip(sprints.OrderBy(s => s.StartDate)
                    .ThenBy(s => s.EndDate)
                    .ThenBy(s => s.Name.Value)
                    .ThenBy(s => s.CreatedDateTime))
                .Should().AllSatisfy(p => AssertResponse(p.First, p.Second));
        }

        internal static void AssertCreation(
            ProjectSprintAggregate sprint,
            CreateProjectSprintRequest request)
        {
            sprint.Name.Value.Should().Be(request.Name);
            sprint.StartDate.Should().Be(request.StartDate);
            sprint.EndDate.Should().Be(request.EndDate);
            sprint.Project.Id.Value.Should().Be(request.ProjectId);

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }

        internal static void AssertAddedStage(
            ProjectSprintAggregate sprint,
            AddStageToProjectSprintRequest request)
        {
            sprint.Id.Value.Should().Be(request.Id);
            var newStage = sprint.Stages.Single(s => s.Name.Value == request.Name);
            newStage.CustomOrderPosition.Value.Should().Be(request.OnTop ? 0 : sprint.Stages.Count - 1);

            var customOrderPosition = 0;
            sprint.Stages
                .OrderBy(s => s.CustomOrderPosition.Value)
                .Should().AllSatisfy(s => s.CustomOrderPosition.Value.Should().Be(customOrderPosition++));

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }

        internal static void AssertUpdated(
            ProjectSprintAggregate sprint,
            ProjectSprintAggregate newSprint,
            UpdateProjectSprintRequest request)
        {
            // unchanged
            newSprint.Id.Value.Should().Be(request.Id);
            newSprint.CreatedDateTime.Should().Be(sprint.CreatedDateTime);
            newSprint.Project.Should().Be(sprint.Project);

            // changed
            newSprint.Name.Value.Should().Be(request.Name);
            newSprint.StartDate.Should().Be(request.StartDate);
            newSprint.EndDate.Should().Be(request.EndDate);
            newSprint.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());

            newSprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            newSprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }

        internal static void AssertUpdatedStage(
            ProjectSprintAggregate sprint,
            UpdateStageFromProjectSprintRequest request)
        {
            sprint.Id.Value.Should().Be(request.Id);
            var stage = sprint.Stages.Single(s => s.Id.Value == request.StageId);
            stage.Name.Value.Should().Be(request.Name);

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }

        internal static void AssertRemovedStage(
            ProjectSprintAggregate sprint,
            RemoveStageFromProjectSprintRequest request)
        {
            sprint.Id.Value.Should().Be(request.Id);
            sprint.Stages.Should().NotContain(s => s.Id.Value == request.StageId);
            
            var customOrderPosition = 0;
            sprint.Stages
                .OrderBy(s => s.CustomOrderPosition.Value)
                .Should().AllSatisfy(s => s.CustomOrderPosition.Value.Should().Be(customOrderPosition++));

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }

        private static void AssertResponse(
            GetCollectionProjectSprintResponse.ProjectSprintResponse response,
            ProjectSprintAggregate projectSprint)
        {
            response.Id.Should().Be(projectSprint.Id.Value);
            response.Name.Should().Be(projectSprint.Name.Value);
            response.StartDate.Should().Be(projectSprint.StartDate);
            response.EndDate.Should().Be(projectSprint.EndDate);
        }
    }
}