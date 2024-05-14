using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

[Route("Projects/Tasks")]
public sealed class ProjectTaskController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
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

    [HttpDelete("{id:guid}")]
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