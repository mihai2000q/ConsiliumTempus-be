using ConsiliumTempus.Domain.Workspace;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetOverview;

public sealed record GetOverviewWorkspaceQuery(Guid Id)
    : IRequest<ErrorOr<WorkspaceAggregate>>;