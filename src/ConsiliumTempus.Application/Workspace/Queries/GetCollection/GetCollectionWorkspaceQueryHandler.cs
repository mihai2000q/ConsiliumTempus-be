using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed class GetCollectionWorkspaceQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<GetCollectionWorkspaceQuery, ErrorOr<GetCollectionWorkspaceResult>>
{
    public async Task<ErrorOr<GetCollectionWorkspaceResult>> Handle(GetCollectionWorkspaceQuery workspaceQuery, 
        CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUser(cancellationToken);
        if (user is null) return Errors.User.NotFound;
        return new GetCollectionWorkspaceResult(
            await workspaceRepository.GetListForUser(
                user, 
                cancellationToken));
    }
}