using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;

public sealed record UpdateOverviewWorkspaceCommand(
    Guid Id,
    string Description)
    : IRequest<ErrorOr<UpdateOverviewWorkspaceResult>>;