using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Commands.UpdateCurrent;

public sealed class UpdateCurrentUserCommandHandler(ICurrentUserProvider currentUserProvider)
    : IRequestHandler<UpdateCurrentUserCommand, ErrorOr<UpdateCurrentUserResult>>
{
    public async Task<ErrorOr<UpdateCurrentUserResult>> Handle(UpdateCurrentUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUser(cancellationToken);

        if (user is null) return Errors.User.NotFound;

        user.Update(
            FirstName.Create(command.FirstName.Capitalize()),
            LastName.Create(command.LastName.Capitalize()),
            command.Role.IfNotNull(Role.Create),
            command.DateOfBirth);

        return new UpdateCurrentUserResult();
    }
}