using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Queries.GetCollection;

public sealed record GetCollectionProjectQuery(
    int? PageSize,
    int? CurrentPage,
    List<string> OrderBy,
    List<string> Search,
    Guid? WorkspaceId)
    : IRequest<ErrorOr<GetCollectionProjectResult>>;