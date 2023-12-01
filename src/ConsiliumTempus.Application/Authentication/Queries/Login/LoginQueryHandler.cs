using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<LoginResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IScrambler _scrambler;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginQueryHandler(IUserRepository userRepository, IScrambler scrambler, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _scrambler = scrambler;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<LoginResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var user = _userRepository.GetUserByEmail(query.Email);
        if (user is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var isPasswordEqual = _scrambler.VerifyPassword(query.Password, user.Password);
        if (!isPasswordEqual)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new LoginResult(token);
    }
}