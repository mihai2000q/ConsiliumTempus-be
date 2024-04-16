using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;

public sealed record GetCollectionProjectForWorkspaceQuery(
    Guid WorkspaceId)
    : IRequest<ErrorOr<GetCollectionProjectForWorkspaceResult>>;