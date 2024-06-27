using ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Create;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Get;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetStages;
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
            AssertUserResponse(response.CreatedBy, sprint.Audit.CreatedBy);
            response.CreatedDateTime.Should().Be(sprint.Audit.CreatedDateTime);
            AssertUserResponse(response.UpdatedBy, sprint.Audit.UpdatedBy);
            response.UpdatedDateTime.Should().Be(sprint.Audit.UpdatedDateTime);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionProjectSprintResponse response,
            IReadOnlyList<ProjectSprintAggregate> sprints,
            int totalCount)
        {
            response.Sprints.Should().HaveCount(sprints.Count);
            response.Sprints
                .Zip(sprints.OrderByDescending(s => s.StartDate)
                    .ThenByDescending(s => s.EndDate)
                    .ThenByDescending(s => s.Name.Value)
                    .ThenByDescending(s => s.Audit.CreatedDateTime))
                .Should().AllSatisfy(p => AssertResponse(p.First, p.Second));
            response.TotalCount.Should().Be(totalCount);
        }
        
        internal static void AssertGetStagesResponse(
            GetStagesFromProjectSprintResponse response,
            IReadOnlyList<ProjectStage> stages)
        {
            response.Stages.Should().HaveCount(stages.Count);
            response.Stages
                .Zip(stages)
                .Should().AllSatisfy(p => AssertStageResponse(p.First, p.Second));
        }

        internal static void AssertCreation(
            ProjectSprintAggregate sprint,
            CreateProjectSprintRequest request,
            ProjectAggregate project,
            UserAggregate createdBy,
            DateOnly? previousSprintEndDate)
        {
            sprint.Name.Value.Should().Be(request.Name);
            sprint.StartDate.Should().Be(request.StartDate);
            sprint.EndDate.Should().Be(request.EndDate);
            sprint.Project.Id.Value.Should().Be(request.ProjectId);
            sprint.Audit.ShouldBeCreated(createdBy);

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            
            if (request.KeepPreviousStages)
            {
                if (project.Sprints.Count != 0)
                {
                    sprint.Stages.Should().HaveSameCount(project.Sprints[0].Stages);
                    sprint.Stages
                        .OrderBy(s => s.CustomOrderPosition.Value)
                        .Zip(project.Sprints[0].Stages.OrderBy(s => s.CustomOrderPosition.Value))
                        .Should().AllSatisfy((x) =>
                        {
                            var (newStage, stage) = x;
                            newStage.Name.Should().Be(stage.Name);
                            newStage.CustomOrderPosition.Should().Be(stage.CustomOrderPosition);
                            newStage.Sprint.Should().Be(sprint);
                            newStage.Audit.ShouldBeCreated(createdBy);
                        });
                }
                else
                    sprint.Stages.Should().BeEmpty();
            }
            else
            {
                sprint.Stages.Should().HaveCount(1);
                sprint.Stages[0].Name.Value.Should().Be(Constants.ProjectStage.Names[0]);
            }

            if (project.Sprints.Count != 0)
            {
                if (previousSprintEndDate is null)
                    sprint.Project.Sprints
                        .OrderByDescending(s => s.Audit.CreatedDateTime).ToList()[1]
                        .EndDate
                        .Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
                else
                    sprint.Project.Sprints
                        .OrderByDescending(s => s.Audit.CreatedDateTime).ToList()[1]
                        .EndDate
                        .Should().Be(previousSprintEndDate);
            }

            if (request.ProjectStatus is not null)
            {
                sprint.Project.LatestStatus.Should().NotBeNull();
                sprint.Project.LatestStatus!.Title.Value.Should().Be(request.ProjectStatus.Title);
                sprint.Project.LatestStatus.Status.ToString().ToLower().Should().Be(request.ProjectStatus.Status.ToLower());
                sprint.Project.LatestStatus.Description.Value.Should().Be(request.ProjectStatus.Description);
            }
        }

        internal static void AssertAddedStage(
            ProjectSprintAggregate sprint,
            AddStageToProjectSprintRequest request,
            UserAggregate createdBy)
        {
            sprint.Id.Value.Should().Be(request.Id);
            var newStage = sprint.Stages.Single(s => s.Name.Value == request.Name);
            newStage.CustomOrderPosition.Value.Should().Be(request.OnTop ? 0 : sprint.Stages.Count - 1);

            newStage.Audit.ShouldBeCreated(createdBy);

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
            UserAggregate updatedby)
        {
            // unchanged
            newSprint.Id.Value.Should().Be(request.Id);
            newSprint.Audit.CreatedDateTime.Should().Be(sprint.Audit.CreatedDateTime);
            newSprint.Project.Should().Be(sprint.Project);

            // changed
            newSprint.Name.Value.Should().Be(request.Name);
            newSprint.StartDate.Should().Be(request.StartDate);
            newSprint.EndDate.Should().Be(request.EndDate);
            newSprint.Audit.ShouldBeUpdated(updatedby);

            newSprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            newSprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertUpdatedStage(
            ProjectSprintAggregate sprint,
            UpdateStageFromProjectSprintRequest request,
            UserAggregate updatedBy)
        {
            sprint.Id.Value.Should().Be(request.Id);
            var stage = sprint.Stages.Single(s => s.Id.Value == request.StageId);
            stage.Name.Value.Should().Be(request.Name);
            
            stage.Audit.ShouldBeUpdated(updatedBy);

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
            response.CreatedDateTime.Should().Be(projectSprint.Audit.CreatedDateTime);
        }
        
        private static void AssertStageResponse(
            GetStagesFromProjectSprintResponse.ProjectStageResponse response,
            ProjectStage projectStage)
        {
            response.Id.Should().Be(projectStage.Id.Value);
            response.Name.Should().Be(projectStage.Name.Value);
        }
        
        private static void AssertUserResponse(
            GetProjectSprintResponse.UserResponse? response,
            UserAggregate? user)
        {
            if (user is null)
            {
                response.Should().BeNull();
                return;
            }
            
            response!.Id.Should().Be(user.Id.Value);
            response.Name.Should().Be(user.FirstName.Value + " " + user.LastName.Value);
            response.Email.Should().Be(user.Credentials.Email);
        }
    }
}