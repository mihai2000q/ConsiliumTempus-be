using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.Dto;
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
            getResult => Ok(Mapper.Map<WorkspaceDto>(getResult)),
            Problem
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetCollection(CancellationToken cancellationToken)
    {
        var query = new GetCollectionWorkspaceQuery();
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result.Select(w => Mapper.Map<WorkspaceDto>(w)));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWorkspaceRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return Ok(Mapper.Map<WorkspaceDto>(result));
    }

    [HasPermission(Permissions.UpdateWorkspace)]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateWorkspaceCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateResult => Ok(Mapper.Map<WorkspaceDto>(updateResult)),
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