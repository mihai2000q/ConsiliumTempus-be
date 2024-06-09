using ConsiliumTempus.Api.Contracts.Project.AddStatus;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetOverview;
using ConsiliumTempus.Api.Contracts.Project.GetStatuses;
using ConsiliumTempus.Api.Contracts.Project.RemoveStatus;
using ConsiliumTempus.Api.Contracts.Project.Update;
using ConsiliumTempus.Api.Contracts.Project.UpdateOverview;
using ConsiliumTempus.Api.Contracts.Project.UpdateStatus;
using ConsiliumTempus.Application.Project.Commands.AddStatus;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.RemoveStatus;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.Project.Commands.UpdateStatus;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Application.Project.Queries.GetStatuses;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Project
    {
        internal static bool AssertGetProjectQuery(
            GetProjectQuery query,
            GetProjectRequest request)
        {
            query.Id.Should().Be(request.Id);

            return true;
        }

        internal static bool AssertGetOverviewProjectQuery(
            GetOverviewProjectQuery query,
            GetOverviewProjectRequest request)
        {
            query.Id.Should().Be(request.Id);

            return true;
        }

        internal static bool AssertGetCollectionProjectQuery(
            GetCollectionProjectQuery query,
            GetCollectionProjectRequest request)
        {
            query.PageSize.Should().Be(request.PageSize);
            query.CurrentPage.Should().Be(request.CurrentPage);
            query.OrderBy.Should().BeEquivalentTo(request.OrderBy);
            query.WorkspaceId.Should().Be(request.WorkspaceId);
            query.Search.Should().BeEquivalentTo(request.Search);

            return true;
        }
        
        internal static bool AssertGetStatusesFromProjectQuery(
            GetStatusesFromProjectQuery query,
            GetStatusesFromProjectRequest request)
        {
            query.Id.Should().Be(request.Id);

            return true;
        }

        internal static bool AssertCreateCommand(
            CreateProjectCommand command,
            CreateProjectRequest request)
        {
            command.WorkspaceId.Should().Be(request.WorkspaceId);
            command.Name.Should().Be(request.Name);
            command.IsPrivate.Should().Be(request.IsPrivate);

            return true;
        }

        internal static bool AssertAddStatusCommand(
            AddStatusToProjectCommand command,
            AddStatusToProjectRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Title.Should().Be(request.Title);
            command.Status.Should().Be(request.Status);
            command.Description.Should().Be(request.Description);

            return true;
        }

        internal static bool AssertUpdateCommand(
            UpdateProjectCommand command,
            UpdateProjectRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);
            command.IsFavorite.Should().Be(request.IsFavorite);

            return true;
        }

        internal static bool AssertUpdateOverviewCommand(
            UpdateOverviewProjectCommand command,
            UpdateOverviewProjectRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Description.Should().Be(request.Description);

            return true;
        }

        internal static bool AssertUpdateStatusCommand(
            UpdateStatusFromProjectCommand command,
            UpdateStatusFromProjectRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.StatusId.Should().Be(request.StatusId);
            command.Title.Should().Be(request.Title);
            command.Status.Should().Be(request.Status);
            command.Description.Should().Be(request.Description);

            return true;
        }

        internal static bool AssertDeleteCommand(
            DeleteProjectCommand command,
            DeleteProjectRequest request)
        {
            command.Id.Should().Be(request.Id);

            return true;
        }

        internal static bool AssertRemoveStatusCommand(
            RemoveStatusFromProjectCommand command,
            RemoveStatusFromProjectRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.StatusId.Should().Be(request.StatusId);

            return true;
        }

        internal static void AssertGetProjectResponse(
            GetProjectResponse response,
            GetProjectResult result)
        {
            response.Name.Should().Be(result.Project.Name.Value);
            response.IsFavorite.Should().Be(result.Project.IsFavorite(result.CurrentUser));
            response.Lifecycle.Should().Be(result.Project.Lifecycle.ToString());
            AssertUserResponse(response.Owner, result.Project.Owner);
            response.IsPrivate.Should().Be(result.Project.IsPrivate.Value);
            if (result.Project.Statuses.Count == 0)
                response.LatestStatus.Should().BeNull();
            else
                AssertProjectStatusResponse(response.LatestStatus!, result.Project.Statuses[0]);
        }

        internal static void AssertGetOverviewProjectResponse(
            GetOverviewProjectResponse response,
            GetOverviewProjectResult result)
        {
            response.Description.Should().Be(result.Description.Value);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionProjectResponse response,
            GetCollectionProjectResult result)
        {
            response.Projects.Zip(result.Projects)
                .Should().AllSatisfy(p => AssertProjectResponse(p.First, p.Second, result.CurrentUser));
            response.TotalCount.Should().Be(result.TotalCount);
        }
        
        internal static void AssertGetStatusesResponse(
            GetStatusesFromProjectResponse response,
            GetStatusesFromProjectResult result)
        {
            response.Statuses.Zip(result.Statuses)
                .Should().AllSatisfy(p => AssertProjectStatusResponse(p.First, p.Second));
            response.TotalCount.Should().Be(result.TotalCount);
        }

        private static void AssertProjectStatusResponse(
            GetProjectResponse.ProjectStatusResponse response,
            ProjectStatus projectStatus)
        {
            response.Id.Should().Be(projectStatus.Id.Value);
            response.Title.Should().Be(projectStatus.Title.Value);
            response.Status.Should().Be(projectStatus.Status.ToString());
            AssertUserResponse(response.CreatedBy, projectStatus.Audit.CreatedBy);
            response.CreatedDateTime.Should().Be(projectStatus.Audit.CreatedDateTime);
            AssertUserResponse(response.UpdatedBy, projectStatus.Audit.UpdatedBy);
            response.UpdatedDateTime.Should().Be(projectStatus.Audit.UpdatedDateTime);
        }

        private static void AssertUserResponse(
            GetProjectResponse.UserResponse? userResponse,
            UserAggregate? user)
        {
            if (userResponse is null) user.Should().BeNull();
            if (user is null) userResponse.Should().BeNull();

            userResponse!.Id.Should().Be(user!.Id.Value);
            userResponse.Name.Should().Be(user.FirstName.Value + " " + user.LastName.Value);
            userResponse.Email.Should().Be(user.Credentials.Email);
        }
        
        private static void AssertProjectResponse(
            GetCollectionProjectResponse.ProjectResponse response,
            ProjectAggregate project,
            UserAggregate currentUser)
        {
            response.Id.Should().Be(project.Id.Value);
            response.Name.Should().Be(project.Name.Value);
            response.Description.Should().Be(project.Description.Value);
            response.IsFavorite.Should().Be(project.IsFavorite(currentUser));
            response.Lifecycle.Should().Be(project.Lifecycle.ToString());
            AssertUserResponse(response.Owner, project.Owner);
            response.IsPrivate.Should().Be(project.IsPrivate.Value);
            if (project.Statuses.Count == 0)
                response.LatestStatus.Should().BeNull();
            else
                AssertProjectStatusResponse(response.LatestStatus!, project.Statuses[0]);
        }
        
        private static void AssertUserResponse(
            GetCollectionProjectResponse.UserResponse userResponse,
            UserAggregate user)
        {
            userResponse.Id.Should().Be(user.Id.Value);
            userResponse.Name.Should().Be(user.FirstName.Value + " " + user.LastName.Value);
            userResponse.Email.Should().Be(user.Credentials.Email);
        }

        private static void AssertProjectStatusResponse(
            GetCollectionProjectResponse.ProjectStatusResponse response,
            ProjectStatus projectStatus)
        {
            response.Id.Should().Be(projectStatus.Id.Value);
            response.Status.Should().Be(projectStatus.Status.ToString());
            response.UpdatedDateTime.Should().Be(projectStatus.Audit.UpdatedDateTime);
        }

        private static void AssertProjectStatusResponse(
            GetStatusesFromProjectResponse.ProjectStatusResponse response,
            ProjectStatus projectStatus)
        {
            response.Id.Should().Be(projectStatus.Id.Value);
            response.Title.Should().Be(projectStatus.Title.Value);
            response.Status.Should().Be(projectStatus.Status.ToString());
            response.Description.Should().Be(projectStatus.Description.Value);
            AssertUserResponse(response.CreatedBy, projectStatus.Audit.CreatedBy);
            response.CreatedDateTime.Should().Be(projectStatus.Audit.CreatedDateTime);
            AssertUserResponse(response.UpdatedBy, projectStatus.Audit.UpdatedBy);
            response.UpdatedDateTime.Should().Be(projectStatus.Audit.UpdatedDateTime);
        }

        private static void AssertUserResponse(
            GetStatusesFromProjectResponse.UserResponse? userResponse,
            UserAggregate? user)
        {
            if (userResponse is null) user.Should().BeNull();
            if (user is null) userResponse.Should().BeNull();

            userResponse!.Id.Should().Be(user!.Id.Value);
            userResponse.Name.Should().Be(user.FirstName.Value + " " + user.LastName.Value);
            userResponse.Email.Should().Be(user.Credentials.Email);
        }
    }
}