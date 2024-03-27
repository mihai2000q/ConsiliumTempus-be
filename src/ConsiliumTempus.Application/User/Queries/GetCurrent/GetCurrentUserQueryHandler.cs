using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.User;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Queries.GetCurrent;

public sealed class GetCurrentUserQueryHandler(ICurrentUserProvider currentUserProvider) 
    : IRequestHandler<GetCurrentUserQuery, ErrorOr<UserAggregate>>
{
    public async Task<ErrorOr<UserAggregate>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        return await currentUserProvider.GetCurrentUser(cancellationToken);
    }
}