using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Delete;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Update;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Delete;
using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Update;
using ConsiliumTempus.Domain.Common.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

[Route("Projects/Stages")]
public sealed class ProjectStageController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HasPermission(Permissions.CreateProjectStage)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectStageRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateProjectStageCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            createResult => Ok(Mapper.Map<CreateProjectStageResponse>(createResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateProjectStage)]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateProjectStageRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateProjectStageCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateResult => Ok(Mapper.Map<UpdateProjectStageResponse>(updateResult)),
            Problem
        );
    }

    [HasPermission(Permissions.DeleteProjectStage)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteProjectStageCommand(id);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            deleteResult => Ok(Mapper.Map<DeleteProjectStageResponse>(deleteResult)),
            Problem
        );
    }
}