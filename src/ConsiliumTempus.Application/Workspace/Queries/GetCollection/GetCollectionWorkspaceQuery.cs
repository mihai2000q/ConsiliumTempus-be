using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed record GetCollectionWorkspaceQuery(
    int? PageSize,
    int? CurrentPage,
    string? Order,
    string? Name) 
    : IRequest<ErrorOr<GetCollectionWorkspaceResult>>;