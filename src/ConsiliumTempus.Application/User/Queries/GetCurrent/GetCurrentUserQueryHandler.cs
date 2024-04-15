using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Queries.GetCurrent;

public sealed class GetCurrentUserQueryHandler(ICurrentUserProvider currentUserProvider)
    : IRequestHandler<GetCurrentUserQuery, ErrorOr<UserAggregate>>
{
    public async Task<ErrorOr<UserAggregate>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUser(cancellationToken);
        return user is null ? Errors.User.NotFound : user;
    }
}