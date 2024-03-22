using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Commands.Update;

public sealed class UpdateUserCommandHandler(ICurrentUserProvider currentUserProvider)
    : IRequestHandler<UpdateUserCommand, ErrorOr<UpdateUserResult>>
{
    public async Task<ErrorOr<UpdateUserResult>> Handle(UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUser(cancellationToken);
        
        user.Update(
            FirstName.Create(command.FirstName.Capitalize()),
            LastName.Create(command.LastName.Capitalize()),
            command.Role is null ? null : Role.Create(command.Role),
            command.DateOfBirth);

        return new UpdateUserResult(user);
    }
}