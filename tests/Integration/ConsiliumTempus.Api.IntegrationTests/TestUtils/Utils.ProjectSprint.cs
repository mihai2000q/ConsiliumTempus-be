using ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Create;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Get;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectSprint.RemoveStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Update;
using ConsiliumTempus.Api.Contracts.ProjectSprint.UpdateStage;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.User;

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
            response.Stages
                .Zip(sprint.Stages)
                .Should().AllSatisfy(x => AssertProjectStageResponse(x.First, x.Second));
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionProjectSprintResponse response,
            IReadOnlyList<ProjectSprintAggregate> sprints)
        {
            response.Sprints.Should().HaveCount(sprints.Count);
            response.Sprints
                .Zip(sprints.OrderByDescending(s => s.StartDate)
                    .ThenByDescending(s => s.EndDate)
                    .ThenByDescending(s => s.Name.Value)
                    .ThenByDescending(s => s.Audit.CreatedDateTime))
                .Should().AllSatisfy(p => AssertResponse(p.First, p.Second));
        }

        internal static void AssertCreation(
            ProjectSprintAggregate sprint,
            CreateProjectSprintRequest request,
            ProjectAggregate project,
            UserAggregate user,
            DateOnly? previousSprintEndDate)
        {
            sprint.Name.Value.Should().Be(request.Name);
            sprint.StartDate.Should().Be(request.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow));
            sprint.EndDate.Should().Be(request.EndDate);
            sprint.Project.Id.Value.Should().Be(request.ProjectId);
            if (request.KeepPreviousStages)
            {
                if (project.Sprints.Count != 0)
                    sprint.Stages.Should().BeEquivalentTo(project.Sprints[0].Stages);
                else
                    sprint.Stages.Should().BeEmpty();
            }
            else
            {
                sprint.Stages.Should().HaveCount(1);
                sprint.Stages[0].Name.Value.Should().Be(Constants.ProjectStage.Names[0]);
            }

            if (project.Sprints.Count != 0)
                if (previousSprintEndDate is null)
                    sprint.Project.Sprints.OrderByDescending(s => s.StartDate).ToList()[1].EndDate
                        .Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
                else
                    sprint.Project.Sprints.OrderByDescending(s => s.StartDate).ToList()[1].EndDate
                        .Should().Be(previousSprintEndDate);

            sprint.Audit.CreatedBy.Should().Be(user);
            sprint.Audit.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            sprint.Audit.UpdatedBy.Should().Be(user);
            sprint.Audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
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

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertUpdated(
            ProjectSprintAggregate sprint,
            ProjectSprintAggregate newSprint,
            UpdateProjectSprintRequest request,
            UserAggregate user)
        {
            // unchanged
            newSprint.Id.Value.Should().Be(request.Id);
            newSprint.Audit.CreatedDateTime.Should().Be(sprint.Audit.CreatedDateTime);
            newSprint.Project.Should().Be(sprint.Project);

            // changed
            newSprint.Name.Value.Should().Be(request.Name);
            newSprint.StartDate.Should().Be(request.StartDate);
            newSprint.EndDate.Should().Be(request.EndDate);
            newSprint.Audit.UpdatedBy.Should().Be(user);
            newSprint.Audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            newSprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            newSprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertUpdatedStage(
            ProjectSprintAggregate sprint,
            UpdateStageFromProjectSprintRequest request)
        {
            sprint.Id.Value.Should().Be(request.Id);
            var stage = sprint.Stages.Single(s => s.Id.Value == request.StageId);
            stage.Name.Value.Should().Be(request.Name);

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
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

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
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

        private static void AssertProjectStageResponse(
            GetProjectSprintResponse.ProjectStageResponse response,
            ProjectStage projectStage)
        {
            response.Id.Should().Be(projectStage.Id.Value);
            response.Name.Should().Be(projectStage.Name.Value);
        }
    }
}