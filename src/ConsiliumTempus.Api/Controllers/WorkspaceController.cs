using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Workspace.AcceptInvitation;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.GetInvitations;
using ConsiliumTempus.Api.Contracts.Workspace.GetOverview;
using ConsiliumTempus.Api.Contracts.Workspace.InviteCollaborator;
using ConsiliumTempus.Api.Contracts.Workspace.Leave;
using ConsiliumTempus.Api.Contracts.Workspace.RejectInvitation;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateCollaborator;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateFavorites;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateOverview;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateOwner;
using ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;
using ConsiliumTempus.Application.Workspace.Commands.Leave;
using ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Commands.UpdateCollaborator;
using ConsiliumTempus.Application.Workspace.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOwner;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Application.Workspace.Queries.GetInvitations;
using ConsiliumTempus.Application.Workspace.Queries.GetOverview;
using ConsiliumTempus.Domain.Common.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

public sealed class WorkspaceController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HasPermission(Permissions.ReadWorkspace)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(GetWorkspaceRequest request, CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetWorkspaceQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getResult => Ok(Mapper.From(getResult)
                .AddParameters(WorkspaceMappingConfig.CurrentUser, getResult.CurrentUser)
                .AdaptToType<GetWorkspaceResponse>()),
            Problem
        );
    }

    [HasPermission(Permissions.ReadOverviewWorkspace)]
    [HttpGet("Overview/{id:guid}")]
    public async Task<IActionResult> GetOverview(GetOverviewWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetOverviewWorkspaceQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getOverviewResult => Ok(Mapper.Map<GetOverviewWorkspaceResponse>(getOverviewResult)),
            Problem
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetCollection(GetCollectionWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetCollectionWorkspaceQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getCollectionResult => Ok(Mapper.From(getCollectionResult)
                .AddParameters(WorkspaceMappingConfig.CurrentUser, getCollectionResult.CurrentUser)
                .AdaptToType<GetCollectionWorkspaceResponse>()),
            Problem
        );
    }

    [HasPermission(Permissions.ReadCollaboratorsFromWorkspace)]
    [HttpGet("{id:guid}/Collaborators")]
    public async Task<IActionResult> GetCollaborators(GetCollaboratorsFromWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetCollaboratorsFromWorkspaceQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getCollaborators => Ok(Mapper.Map<GetCollaboratorsFromWorkspaceResponse>(getCollaborators)),
            Problem
        );
    }

    [HasPermission(Permissions.ReadInvitationsFromWorkspace)]
    [HttpGet("Invitations")]
    public async Task<IActionResult> GetInvitations(GetInvitationsWorkspaceRequest request, 
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetInvitationsWorkspaceQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getInvitationsResult => Ok(Mapper.Map<GetInvitationsWorkspaceResponse>(getInvitationsResult)),
            Problem
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWorkspaceRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            createResult => Ok(Mapper.Map<CreateWorkspaceResponse>(createResult)),
            Problem
        );
    }

    [HasPermission(Permissions.InviteCollaboratorToWorkspace)]
    [HttpPost("Invite-Collaborator")]
    public async Task<IActionResult> InviteCollaborator(InviteCollaboratorToWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<InviteCollaboratorToWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            inviteCollaboratorResult => Ok(Mapper.Map<InviteCollaboratorToWorkspaceResponse>(inviteCollaboratorResult)),
            Problem
        );
    }

    [HttpPost("Accept-Invitation")]
    public async Task<IActionResult> AcceptInvitation(AcceptInvitationToWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<AcceptInvitationToWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            acceptInvitationResult => Ok(Mapper.Map<AcceptInvitationToWorkspaceResponse>(acceptInvitationResult)),
            Problem
        );
    }

    [HttpPost("Reject-Invitation")]
    public async Task<IActionResult> RejectInvitation(RejectInvitationToWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<RejectInvitationToWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            rejectInvitationResult => Ok(Mapper.Map<RejectInvitationToWorkspaceResponse>(rejectInvitationResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateWorkspace)]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateWorkspaceRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateResult => Ok(Mapper.Map<UpdateWorkspaceResponse>(updateResult)),
            Problem
        );
    }
    
    [HasPermission(Permissions.UpdateCollaboratorFromWorkspace)]
    [HttpPut("Collaborators")]
    public async Task<IActionResult> UpdateCollaborator(UpdateCollaboratorFromWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateCollaboratorFromWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateCollaboratorResult => Ok(Mapper.Map<UpdateCollaboratorFromWorkspaceResponse>(updateCollaboratorResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateFavoritesWorkspace)]
    [HttpPut("Favorites")]
    public async Task<IActionResult> UpdateFavorites(UpdateFavoritesWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateFavoritesWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateFavoritesResult => Ok(Mapper.Map<UpdateFavoritesWorkspaceResponse>(updateFavoritesResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateOverviewWorkspace)]
    [HttpPut("Overview")]
    public async Task<IActionResult> UpdateOverview(UpdateOverviewWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateOverviewWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateOverviewResult => Ok(Mapper.Map<UpdateOverviewWorkspaceResponse>(updateOverviewResult)),
            Problem
        );
    }

    [HasWorkspaceAuthorization(WorkspaceAuthorizationLevel.IsWorkspaceOwner)]
    [HttpPut("Owner")]
    public async Task<IActionResult> UpdateOwner(UpdateOwnerWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateOwnerWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateOwnerResult => Ok(Mapper.Map<UpdateOwnerWorkspaceResponse>(updateOwnerResult)),
            Problem
        );
    }

    [HasPermission(Permissions.DeleteWorkspace)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(DeleteWorkspaceRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<DeleteWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            deleteResult => Ok(Mapper.Map<DeleteWorkspaceResponse>(deleteResult)),
            Problem
        );
    }

    [HasWorkspaceAuthorization(WorkspaceAuthorizationLevel.IsCollaborator)]
    [HttpDelete("{id:guid}/Leave")]
    public async Task<IActionResult> Leave(LeaveWorkspaceRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<LeaveWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            leaveResult => Ok(Mapper.Map<LeaveWorkspaceResponse>(leaveResult)),
            Problem
        );
    }
}