using ConsiliumTempus.Api.Contracts.User.DeleteCurrent;
using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.GetCurrent;
using ConsiliumTempus.Api.Contracts.User.UpdateCurrent;
using ConsiliumTempus.Application.User.Commands.DeleteCurrent;
using ConsiliumTempus.Application.User.Commands.UpdateCurrent;
using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Application.User.Queries.GetCurrent;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

public sealed class UserController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(GetUserRequest request, CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetUserQuery>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getResult => Ok(Mapper.Map<GetUserResponse>(getResult)),
            Problem
        );
    }

    [HttpGet("Current")]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserQuery();
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            getCurrentResult => Ok(Mapper.Map<GetCurrentUserResponse>(getCurrentResult)),
            Problem
        );
    }

    [HttpPut("Current")]
    public async Task<IActionResult> UpdateCurrent(UpdateCurrentUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateCurrentUserCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateResult => Ok(Mapper.Map<UpdateCurrentUserResponse>(updateResult)),
            Problem
        );
    }

    [HttpDelete("Current")]
    public async Task<IActionResult> DeleteCurrent(CancellationToken cancellationToken)
    {
        var command = new DeleteCurrentUserCommand();
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            deleteResult => Ok(Mapper.Map<DeleteCurrentUserResponse>(deleteResult)),
            Problem
        );
    }
}