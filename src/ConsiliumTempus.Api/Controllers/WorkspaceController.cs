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
public class WorkspaceController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HasPermission(Permissions.ReadWorkspace)]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(GetWorkspaceRequest request)
    {
        var query = Mapper.Map<GetWorkspaceQuery>(request);
        var result = await Mediator.Send(query);

        return result.Match(
            getResult => Ok(Mapper.Map<WorkspaceDto>(getResult)),
            Problem
        );
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateWorkspaceRequest request)
    {
        var token = GetToken();
        var command = Mapper.From(request)
            .AddParameters(WorkspaceMappingConfig.Token, token)
            .AdaptToType<CreateWorkspaceCommand>();
        var result = await Mediator.Send(command);

        return result.Match(
            createResult => Ok(Mapper.Map<WorkspaceDto>(createResult)),
            Problem
        );
    }
}