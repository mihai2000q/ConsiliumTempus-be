using ConsiliumTempus.Api.Contracts.Workspace.AcceptInvitation;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.GetInvitations;
using ConsiliumTempus.Api.Contracts.Workspace.GetOverview;
using ConsiliumTempus.Api.Contracts.Workspace.InviteCollaborator;
using ConsiliumTempus.Api.Contracts.Workspace.RejectInvitation;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateFavorites;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateOverview;
using ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;
using ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Application.Workspace.Queries.GetInvitations;
using ConsiliumTempus.Application.Workspace.Queries.GetOverview;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.Entities;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Workspace
    {
        internal static bool AssertGetQuery(
            GetWorkspaceQuery query,
            GetWorkspaceRequest request)
        {
            query.Id.Should().Be(request.Id);

            return true;
        }

        internal static bool AssertGetOverviewQuery(
            GetOverviewWorkspaceQuery query,
            GetOverviewWorkspaceRequest request)
        {
            query.Id.Should().Be(request.Id);

            return true;
        }

        internal static bool AssertGetCollectionQuery(
            GetCollectionWorkspaceQuery query,
            GetCollectionWorkspaceRequest request)
        {
            query.IsPersonalWorkspaceFirst.Should().Be(request.IsPersonalWorkspaceFirst);
            query.PageSize.Should().Be(request.PageSize);
            query.CurrentPage.Should().Be(request.CurrentPage);
            query.OrderBy.Should().BeEquivalentTo(request.OrderBy);
            query.Search.Should().BeEquivalentTo(request.Search);

            return true;
        }

        internal static bool AssertGetCollaboratorsQuery(
            GetCollaboratorsFromWorkspaceQuery query,
            GetCollaboratorsFromWorkspaceRequest request)
        {
            query.Id.Should().Be(request.Id);
            query.SearchValue.Should().Be(request.SearchValue);

            return true;
        }

        internal static bool AssertGetInvitationsQuery(
            GetInvitationsWorkspaceQuery query,
            GetInvitationsWorkspaceRequest request)
        {
            query.IsSender.Should().Be(request.IsSender);
            query.WorkspaceId.Should().Be(request.WorkspaceId);
            query.PageSize.Should().Be(request.PageSize);
            query.CurrentPage.Should().Be(request.CurrentPage);

            return true;
        }

        internal static bool AssertCreateCommand(
            CreateWorkspaceCommand command,
            CreateWorkspaceRequest request)
        {
            command.Name.Should().Be(request.Name);

            return true;
        }

        internal static bool AssertInviteCollaboratorCommand(
            InviteCollaboratorToWorkspaceCommand command,
            InviteCollaboratorToWorkspaceRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Email.Should().Be(request.Email);

            return true;
        }

        internal static bool AssertAcceptInvitationCommand(
            AcceptInvitationToWorkspaceCommand command,
            AcceptInvitationToWorkspaceRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.InvitationId.Should().Be(request.InvitationId);

            return true;
        }

        internal static bool AssertRejectInvitationCommand(
            RejectInvitationToWorkspaceCommand command,
            RejectInvitationToWorkspaceRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.InvitationId.Should().Be(request.InvitationId);

            return true;
        }

        internal static bool AssertUpdateCommand(
            UpdateWorkspaceCommand command,
            UpdateWorkspaceRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);

            return true;
        }

        internal static bool AssertUpdateFavoritesCommand(
            UpdateFavoritesWorkspaceCommand command,
            UpdateFavoritesWorkspaceRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.IsFavorite.Should().Be(request.IsFavorite);

            return true;
        }

        internal static bool AssertUpdateOverviewCommand(
            UpdateOverviewWorkspaceCommand command,
            UpdateOverviewWorkspaceRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Description.Should().Be(request.Description);

            return true;
        }

        internal static bool AssertDeleteCommand(
            DeleteWorkspaceCommand command,
            DeleteWorkspaceRequest request)
        {
            command.Id.Should().Be(request.Id);

            return true;
        }

        internal static void AssertGetResponse(
            GetWorkspaceResponse response,
            GetWorkspaceResult result)
        {
            response.Name.Should().Be(result.Workspace.Name.Value);
            response.IsFavorite.Should().Be(result.Workspace.IsFavorite(result.CurrentUser));
            response.IsPersonal.Should().Be(result.Workspace.IsPersonal.Value);
            AssertUserResponse(response.Owner, result.Workspace.Owner);
        }

        internal static void AssertGetOverviewResponse(
            GetOverviewWorkspaceResponse response,
            WorkspaceAggregate workspace)
        {
            response.Description.Should().Be(workspace.Description.Value);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionWorkspaceResponse response,
            GetCollectionWorkspaceResult result)
        {
            response.Workspaces.Zip(result.Workspaces)
                .Should().AllSatisfy(p => AssertWorkspaceResponse(p.First, p.Second, result.CurrentUser));
            response.TotalCount.Should().Be(result.TotalCount);
        }

        internal static void AssertGetCollaboratorsResponse(
            GetCollaboratorsFromWorkspaceResponse response,
            GetCollaboratorsFromWorkspaceResult result)
        {
            response.Collaborators.Zip(result.Collaborators)
                .Should().AllSatisfy(p => AssertUserResponse(p.First, p.Second));
        }

        internal static void AssertGetInvitationsResponse(
            GetInvitationsWorkspaceResponse response,
            GetInvitationsWorkspaceResult result)
        {
            response.Invitations.Zip(result.Invitations)
                .Should().AllSatisfy(p => AssertWorkspaceInvitationResponse(p.First, p.Second));
            response.TotalCount.Should().Be(result.TotalCount);
        }

        private static void AssertUserResponse(
            GetWorkspaceResponse.UserResponse response,
            UserAggregate user)
        {
            response.Id.Should().Be(user.Id.Value);
            response.Name.Should().Be(user.Name.Value);
            response.Email.Should().Be(user.Credentials.Email);
        }

        private static void AssertWorkspaceResponse(
            GetCollectionWorkspaceResponse.WorkspaceResponse response,
            WorkspaceAggregate workspace,
            UserAggregate currentUser)
        {
            response.Id.Should().Be(workspace.Id.Value);
            response.Name.Should().Be(workspace.Name.Value);
            response.Description.Should().Be(workspace.Description.Value);
            response.IsFavorite.Should().Be(workspace.IsFavorite(currentUser));
            response.IsPersonal.Should().Be(workspace.IsPersonal.Value);
            AssertUserResponse(response.Owner, workspace.Owner);
        }

        private static void AssertUserResponse(
            GetCollectionWorkspaceResponse.UserResponse response,
            UserAggregate user)
        {
            response.Id.Should().Be(user.Id.Value);
            response.Name.Should().Be(user.Name.Value);
            response.Email.Should().Be(user.Credentials.Email);
        }

        private static void AssertUserResponse(
            GetCollaboratorsFromWorkspaceResponse.UserResponse response,
            UserAggregate user)
        {
            response.Id.Should().Be(user.Id.Value);
            response.Name.Should().Be(user.Name.Value);
            response.Email.Should().Be(user.Credentials.Email);
        }

        private static void AssertWorkspaceInvitationResponse(
            GetInvitationsWorkspaceResponse.WorkspaceInvitationResponse response,
            WorkspaceInvitation invitation)
        {
            response.Id.Should().Be(invitation.Id.Value);
            AssertUserResponse(response.Sender, invitation.Sender);
            AssertUserResponse(response.Collaborator, invitation.Collaborator);
            AssertWorkspaceResponse(response.Workspace, invitation.Workspace);
        }

        private static void AssertUserResponse(
            GetInvitationsWorkspaceResponse.UserResponse response,
            UserAggregate user)
        {
            response.Id.Should().Be(user.Id.Value);
            response.Name.Should().Be(user.Name.Value);
            response.Email.Should().Be(user.Credentials.Email);
        }

        private static void AssertWorkspaceResponse(
            GetInvitationsWorkspaceResponse.WorkspaceResponse response,
            WorkspaceAggregate workspace)
        {
            response.Id.Should().Be(workspace.Id.Value);
            response.Name.Should().Be(workspace.Name.Value);
            response.IsPersonal.Should().Be(workspace.IsPersonal.Value);
        }
    }
}