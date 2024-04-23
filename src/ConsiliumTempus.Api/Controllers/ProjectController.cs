using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;
using ConsiliumTempus.Domain.Common.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

public sealed class ProjectController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HasPermission(Permissions.ReadProject)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(GetProjectRequest request, CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetProjectQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            project => Ok(Mapper.Map<GetProjectResponse>(project)),
            Problem
        );
    }

    [HttpGet("User")]
    public async Task<IActionResult> GetCollectionForUser(CancellationToken cancellationToken)
    {
        var query = new GetCollectionProjectForUserQuery();
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getCollectionForUserResult =>
                Ok(Mapper.Map<GetCollectionProjectForUserResponse>(getCollectionForUserResult)),
            Problem
        );
    }

    [HasPermission(Permissions.ReadCollectionProject)]
    [HttpGet]
    public async Task<IActionResult> GetCollection(GetCollectionProjectRequest request, 
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetCollectionProjectQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getCollectionResult => Ok(Mapper.Map<GetCollectionProjectResponse>(getCollectionResult)),
            Problem
        );
    }

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