using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Create;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Delete;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Get;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Update;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.ProjectSprint.Queries.Get;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
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
}