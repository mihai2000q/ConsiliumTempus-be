using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
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
    
    [HasPermission(Permissions.ReadWorkspace)]
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

    [HasPermission(Permissions.UpdateWorkspace)]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateResult => Ok(Mapper.Map<UpdateWorkspaceResponse>(updateResult)),
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
}