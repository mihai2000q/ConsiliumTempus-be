using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;

public sealed record GetCollectionProjectForWorkspaceQuery(
    Guid WorkspaceId,
    string? Name = null,
    bool? IsFavorite = null,
    bool? IsPrivate = null)
    : IRequest<ErrorOr<GetCollectionProjectForWorkspaceResult>>;