using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Refresh;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Application.Authentication.Commands.Login;
using ConsiliumTempus.Application.Authentication.Commands.Refresh;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

[Route("Auth")]
[AllowAnonymous]
public sealed class AuthenticationController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator)
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<RegisterCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            registerResult => Ok(Mapper.Map<RegisterResponse>(registerResult)),
            Problem
        );
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var query = Mapper.Map<LoginCommand>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            loginResult => Ok(Mapper.Map<LoginResponse>(loginResult)),
            Problem
        );
    }

    [HttpPut("Refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request, CancellationToken cancellationToken)
    {
        var query = Mapper.Map<RefreshCommand>(request);
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match(
            refreshResult => Ok(Mapper.Map<RefreshResponse>(refreshResult)),
            Problem
        );
    }
}