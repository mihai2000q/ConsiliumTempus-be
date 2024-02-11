using ConsiliumTempus.Api.Common.Attributes;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.User.Commands.Update;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

public sealed class UserController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
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
}