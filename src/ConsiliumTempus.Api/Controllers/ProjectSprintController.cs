using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Get;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;
using ConsiliumTempus.Application.Project.Entities.Sprint.Queries.Get;
using ConsiliumTempus.Application.Project.Entities.Sprint.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

[Route("Projects/Sprints")]
public sealed class ProjectSprintController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
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