using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetCollection;

public sealed record GetCollectionProjectQuery(
    int? PageSize,
    int? CurrentPage,
    Guid? WorkspaceId,
    string? Name,
    bool? IsFavorite,
    bool? IsPrivate)
    string[]? OrderBy,
    : IRequest<ErrorOr<GetCollectionProjectResult>>;