using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Create;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Delete;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Get;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetStages;
using ConsiliumTempus.Api.Contracts.ProjectSprint.MoveStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.RemoveStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Update;
using ConsiliumTempus.Api.Contracts.ProjectSprint.UpdateStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.MoveStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;
using ConsiliumTempus.Application.ProjectSprint.Queries.Get;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetStages;
using ConsiliumTempus.Domain.Common.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

[Route("Projects/Sprints")]
public sealed class ProjectSprintController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HasPermission(Permissions.ReadProjectSprint)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(GetProjectSprintRequest request, CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetProjectSprintQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            sprint => Ok(Mapper.Map<GetProjectSprintResponse>(sprint)),
            Problem
        );
    }

    [HasPermission(Permissions.ReadCollectionProjectSprint)]
    [HttpGet]
    public async Task<IActionResult> GetCollection(GetCollectionProjectSprintRequest request,
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetCollectionProjectSprintQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getCollectionResult => Ok(Mapper.Map<GetCollectionProjectSprintResponse>(getCollectionResult)),
            Problem
        );
    }

    [HasPermission(Permissions.ReadStagesFromProjectSprint)]
    [HttpGet("{id:guid}/stages")]
    public async Task<IActionResult> GetStages(GetStagesFromProjectSprintRequest request,
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetStagesFromProjectSprintQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getStagesResult => Ok(Mapper.Map<GetStagesFromProjectSprintResponse>(getStagesResult)),
            Problem
        );
    }

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

    [HasPermission(Permissions.AddStageToProjectSprint)]
    [HttpPost("Add-Stage")]
    public async Task<IActionResult> AddStage(AddStageToProjectSprintRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<AddStageToProjectSprintCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            addStageResult => Ok(Mapper.Map<AddStageToProjectSprintResponse>(addStageResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateProjectSprint)]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateProjectSprintRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateProjectSprintCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateResult => Ok(Mapper.Map<UpdateProjectSprintResponse>(updateResult)),
            Problem
        );
    }

    [HasPermission(Permissions.MoveStageFromProjectSprint)]
    [HttpPost("Move-Stage")]
    public async Task<IActionResult> MoveStage(MoveStageFromProjectSprintRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<MoveStageFromProjectSprintCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            moveStageResult => Ok(Mapper.Map<MoveStageFromProjectSprintResponse>(moveStageResult)),
            Problem
        );
    }

    [HasPermission(Permissions.UpdateStageFromProjectSprint)]
    [HttpPut("Update-Stage")]
    public async Task<IActionResult> UpdateStage(UpdateStageFromProjectSprintRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateStageFromProjectSprintCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateStageResult => Ok(Mapper.Map<UpdateStageFromProjectSprintResponse>(updateStageResult)),
            Problem
        );
    }

    [HasPermission(Permissions.DeleteProjectSprint)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(DeleteProjectSprintRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<DeleteProjectSprintCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            deleteResult => Ok(Mapper.Map<DeleteProjectSprintResponse>(deleteResult)),
            Problem
        );
    }

    [HasPermission(Permissions.RemoveStageFromProjectSprint)]
    [HttpDelete("{id:guid}/Remove-Stage/{stageId:guid}")]
    public async Task<IActionResult> RemoveStage(RemoveStageFromProjectSprintRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<RemoveStageFromProjectSprintCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            removeStageResult => Ok(Mapper.Map<RemoveStageFromProjectSprintResponse>(removeStageResult)),
            Problem
        );
    }
}