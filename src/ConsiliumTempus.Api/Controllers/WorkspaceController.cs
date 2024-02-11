using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Domain.Common.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

[Route("api/workspaces")]
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

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateWorkspaceRequest request, CancellationToken cancellationToken)
    {
        var token = GetToken();
        var command = Mapper.From(request)
            .AddParameters(WorkspaceMappingConfig.Token, token)
            .AdaptToType<CreateWorkspaceCommand>();
        var result = await Mediator.Send(command, cancellationToken);

        return Ok(Mapper.Map<WorkspaceDto>(result));
    }
}