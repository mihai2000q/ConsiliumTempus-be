using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetCollection;

public sealed record GetCollectionProjectQuery(
    int? PageSize,
    int? CurrentPage,
    string? Orders,
    Guid? WorkspaceId,
    string? Name,
    bool? IsFavorite,
    bool? IsPrivate)
    : IRequest<ErrorOr<GetCollectionProjectResult>>;