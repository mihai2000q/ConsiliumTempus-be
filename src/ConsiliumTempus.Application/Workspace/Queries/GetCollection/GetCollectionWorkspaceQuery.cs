using ConsiliumTempus.Domain.Workspace;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollection;

public sealed record GetCollectionWorkspaceQuery : IRequest<ErrorOr<List<WorkspaceAggregate>>>;