using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.UpdateOverview;

public sealed record UpdateOverviewProjectCommand(
    Guid Id,
    string Description)
    : IRequest<ErrorOr<UpdateOverviewProjectResult>>;