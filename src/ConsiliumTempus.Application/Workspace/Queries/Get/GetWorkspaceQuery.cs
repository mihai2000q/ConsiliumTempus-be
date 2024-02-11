using ConsiliumTempus.Domain.Workspace;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.Get;

public sealed record GetWorkspaceQuery(Guid Id)
    : IRequest<ErrorOr<WorkspaceAggregate>>;