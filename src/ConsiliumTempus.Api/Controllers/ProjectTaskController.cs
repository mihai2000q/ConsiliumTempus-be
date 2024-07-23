using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectTask.Move;
using ConsiliumTempus.Api.Contracts.ProjectTask.Update;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateIsCompleted;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateOverview;
using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateIsCompleted;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

[Route("Projects/Tasks")]
public sealed class ProjectTaskController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HasPermission(Permissions.ReadProjectTask)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(GetProjectTaskRequest request, CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetProjectTaskQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            task => Ok(Mapper.Map<GetProjectTaskResponse>(task)),
            Problem
        );
    }

    [HasPermission(Permissions.ReadCollectionProjectTask)]
    [HttpGet]
    public async Task<IActionResult> GetCollection(GetCollectionProjectTaskRequest request,
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetCollectionProjectTaskQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getCollectionResult => Ok(Mapper.Map<GetCollectionProjectTaskResponse>(getCollectionResult)),
            Problem
        );
    }

    [HasPermission(Permissions.CreateProjectTask)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectTaskRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateProjectTaskCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            createResult => Ok(Mapper.Map<CreateProjectTaskResponse>(createResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateProjectTask)]
    [HttpPut("Move")]
    public async Task<IActionResult> Move(MoveProjectTaskRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<MoveProjectTaskCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            moveResult => Ok(Mapper.Map<MoveProjectTaskResponse>(moveResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateProjectTask)]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateProjectTaskRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateProjectTaskCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateResult => Ok(Mapper.Map<UpdateProjectTaskResponse>(updateResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateProjectTask)]
    [HttpPut("Is-Completed")]
    public async Task<IActionResult> UpdateIsCompleted(UpdateIsCompletedProjectTaskRequest request, 
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateIsCompletedProjectTaskCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateIsCompletedResult => Ok(Mapper.Map<UpdateIsCompletedProjectTaskResponse>(updateIsCompletedResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateProjectTask)]
    [HttpPut("Overview")]
    public async Task<IActionResult> UpdateOverview(UpdateOverviewProjectTaskRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateOverviewProjectTaskCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateOverviewResult => Ok(Mapper.Map<UpdateOverviewProjectTaskResponse>(updateOverviewResult)),
            Problem
        );
    }

    [HasPermission(Permissions.DeleteProjectTask)]
    [HttpDelete("{id:guid}/from/{stageId:guid}")]
    public async Task<IActionResult> Delete(DeleteProjectTaskRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<DeleteProjectTaskCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            deleteResult => Ok(Mapper.Map<DeleteProjectTaskResponse>(deleteResult)),
            Problem
        );
    }
}