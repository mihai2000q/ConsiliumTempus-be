using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.Orders;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed class GetCollectionWorkspaceQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository)
    : IRequestHandler<GetCollectionWorkspaceQuery, ErrorOr<GetCollectionWorkspaceResult>>
{
    public async Task<ErrorOr<GetCollectionWorkspaceResult>> Handle(GetCollectionWorkspaceQuery query,
        CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUser(cancellationToken);
        if (user is null) return Errors.User.NotFound;

        var order = WorkspaceOrder.Parse(query.Order);
        
        var workspaces = await workspaceRepository.GetListByUser(
            user,
            order,
            cancellationToken);
        
        return new GetCollectionWorkspaceResult(
            workspaces);
    }
}