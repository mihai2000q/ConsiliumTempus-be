using ConsiliumTempus.Domain.Workspace;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed record GetCollectionWorkspaceQuery : IRequest<List<WorkspaceAggregate>>;