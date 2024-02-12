using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Queries.Get;

public sealed class GetUserQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<GetUserQuery, ErrorOr<UserAggregate>>
{
    public async Task<ErrorOr<UserAggregate>> Handle(GetUserQuery query, 
        CancellationToken cancellationToken)
    {
        var user = await userRepository.Get(UserId.Create(query.Id), cancellationToken);
        return user is null ? Errors.User.NotFound : user;
    }
}