using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Queries.GetId;

public sealed class GetUserIdQueryHandler(ICurrentUserProvider currentUserProvider)
    : IRequestHandler<GetUserIdQuery, ErrorOr<UserId>>
{
    public async Task<ErrorOr<UserId>> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
    {
        return (await currentUserProvider.GetCurrentUser(cancellationToken)).Id;
    }
}