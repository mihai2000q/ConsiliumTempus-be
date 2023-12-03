using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.UserAggregate;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<RegisterResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IScrambler _scrambler;
    private readonly IUserRepository _userRepository;
    
    public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IScrambler scrambler, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _scrambler = scrambler;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<RegisterResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetUserByEmail(command.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        var password = _scrambler.HashPassword(command.Password);
        
        var user = User.Create(
            command.FirstName,
            command.LastName,
            command.Email,
            password);
        await _userRepository.Add(user);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new RegisterResult(token);
    }
}