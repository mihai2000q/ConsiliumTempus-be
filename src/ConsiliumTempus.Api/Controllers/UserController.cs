using ConsiliumTempus.Api.Contracts.User.Delete;
using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.GetId;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.User.Commands.Delete;
using ConsiliumTempus.Application.User.Commands.Update;
using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Application.User.Queries.GetCurrent;
using ConsiliumTempus.Application.User.Queries.GetId;
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
            getResult => Ok(Mapper.Map<UserDto>(getResult)),
            Problem
        );
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetId(CancellationToken cancellationToken)
    {
        var query = new GetUserIdQuery();
        var result = await Mediator.Send(query, cancellationToken);
        
        return result.Match(
            getIdResult => Ok(Mapper.Map<GetUserIdResponse>(getIdResult)),
            Problem
        );
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserQuery();
        var result = await Mediator.Send(query, cancellationToken);
        
        return result.Match(
            getCurrentResult => Ok(Mapper.Map<UserDto>(getCurrentResult)),
            Problem
        );
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<UpdateUserCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            updateResult => Ok(Mapper.Map<UserDto>(updateResult)),
            Problem
        );
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand();
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            deleteResult => Ok(Mapper.Map<DeleteUserResponse>(deleteResult)),
            Problem
        );
    }
}