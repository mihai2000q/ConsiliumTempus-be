using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetCollection;

public sealed record GetCollectionProjectQuery(
    Guid? WorkspaceId = null,
    string? Name = null,
    bool? IsFavorite = null,
    bool? IsPrivate = null)
    : IRequest<ErrorOr<GetCollectionProjectResult>>;