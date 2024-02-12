using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.User.Commands.Delete;
using ConsiliumTempus.Application.User.Commands.Update;
using ConsiliumTempus.Application.User.Queries.Get;
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
    
    [IsOwner]
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
    
    [IsOwner]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            deleteResult => Ok(Mapper.Map<UserDto>(deleteResult)),
            Problem
        );
    }
}