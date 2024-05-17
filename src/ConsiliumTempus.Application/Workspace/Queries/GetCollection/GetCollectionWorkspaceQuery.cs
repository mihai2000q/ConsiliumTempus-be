using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed record GetCollectionWorkspaceQuery(
    bool IsPersonalWorkspaceFirst,
    int? PageSize,
    int? CurrentPage,
    string? Orders,
    string? Name) 
    : IRequest<ErrorOr<GetCollectionWorkspaceResult>>;