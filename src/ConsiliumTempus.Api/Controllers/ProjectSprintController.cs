using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;
using ConsiliumTempus.Domain.Common.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

[Route("Api/Projects/Sprints")]
public sealed class ProjectSprintController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HasPermission(Permissions.CreateProjectSprint)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectSprintRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateProjectSprintCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);
        
        return result.Match(
            createResult => Ok(Mapper.Map<CreateProjectSprintResponse>(createResult)),
            Problem
        );
    }
    
    [HasPermission(Permissions.DeleteProjectSprint)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteProjectSprintCommand(id);
        var result = await Mediator.Send(command, cancellationToken);
        
        return result.Match(
            deleteResult => Ok(Mapper.Map<DeleteProjectSprintResponse>(deleteResult)),
            Problem
        );
    }
}