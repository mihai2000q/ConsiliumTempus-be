using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed record GetCollectionWorkspaceQuery(string? Order) 
    : IRequest<ErrorOr<GetCollectionWorkspaceResult>>;