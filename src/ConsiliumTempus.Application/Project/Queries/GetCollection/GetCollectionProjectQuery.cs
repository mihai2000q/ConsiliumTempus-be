using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetCollection;

public sealed record GetCollectionProjectQuery(
    int? PageSize,
    int? CurrentPage,
    string[]? OrderBy,
    string[]? Search,
    Guid? WorkspaceId)
    : IRequest<ErrorOr<GetCollectionProjectResult>>;