using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

[Route("api/workspace")]
public class WorkspaceController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create(WorkspaceCreateRequest request)
    {
        var token = GetToken();
        var command = Mapper.From(request)
            .AddParameters(WorkspaceMappingConfig.Token, token)
            .AdaptToType<WorkspaceCreateCommand>();
        var result = await Mediator.Send(command);

        return result.Match(
            createResult => Ok(Mapper.Map<WorkspaceCreateResponse>(createResult)),
            Problem
        );
    }
}