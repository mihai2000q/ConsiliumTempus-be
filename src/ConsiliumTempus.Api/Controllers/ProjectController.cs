using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Domain.Common.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

public sealed class ProjectController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HasPermission(Permissions.CreateProject)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateProjectCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);
        
        return result.Match(
            createResult => Ok(Mapper.Map<CreateProjectResponse>(createResult)),
            Problem
        );
    }

    [HasPermission(Permissions.DeleteProject)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteProjectCommand(id);
        var result = await Mediator.Send(command, cancellationToken);
        
        return result.Match(
            deleteResult => Ok(Mapper.Map<DeleteProjectResponse>(deleteResult)),
            Problem
        );
    }
}