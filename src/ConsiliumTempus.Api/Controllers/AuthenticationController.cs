using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Authentication.Queries.Login;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Controllers;

[Route("api/auth")]
[AllowAnonymous]
public class AuthenticationController : ApiController
{
    public AuthenticationController(IMapper mapper, ISender mediator) : base(mapper, mediator)
    {
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = Mapper.Map<RegisterCommand>(request);
        var result = await Mediator.Send(command);

        return result.Match(
            registerResult => Ok(Mapper.Map<RegisterResponse>(registerResult)),
            Problem
        );
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = Mapper.Map<LoginQuery>(request);
        var result = await Mediator.Send(query);

        return result.Match(
            loginResult => Ok(Mapper.Map<LoginResponse>(loginResult)),
            Problem
        );
    }
}