using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;

public sealed class GetCollectionProjectForUserQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectRepository projectRepository) 
    : IRequestHandler<GetCollectionProjectForUserQuery, ErrorOr<GetCollectionProjectForUserResult>>
{
    public async Task<ErrorOr<GetCollectionProjectForUserResult>> Handle(GetCollectionProjectForUserQuery query, 
        CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUser(cancellationToken);
        if (user is null) return Errors.User.NotFound;
        return new GetCollectionProjectForUserResult(
            await projectRepository.GetListForUser(user.Id, cancellationToken));
    }
}