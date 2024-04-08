using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed record GetCollectionWorkspaceQuery : IRequest<ErrorOr<GetCollectionWorkspaceResult>>;