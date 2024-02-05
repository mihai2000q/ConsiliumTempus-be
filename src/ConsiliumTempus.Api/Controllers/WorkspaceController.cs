using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Application.Workspace.Command.Create;
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
        var command = Mapper.Map<WorkspaceCreateCommand>(request);
        var result = await Mediator.Send(command);

        return result.Match(
            createResult => Ok(Mapper.Map<WorkspaceCreateResult>(createResult)),
            Problem
        );
    }
}