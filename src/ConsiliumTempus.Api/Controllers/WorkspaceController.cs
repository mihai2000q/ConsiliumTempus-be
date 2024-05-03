using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Queries.Get;
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
            workspace => Ok(Mapper.Map<GetWorkspaceResponse>(workspace)),
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
            getCollectionResult => Ok(Mapper.Map<GetCollectionWorkspaceResponse>(getCollectionResult)),
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
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteWorkspaceCommand(id);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            deleteResult => Ok(Mapper.Map<DeleteWorkspaceResponse>(deleteResult)),
            Problem
        );
    }
}