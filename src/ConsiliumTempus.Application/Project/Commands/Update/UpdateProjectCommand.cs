using ConsiliumTempus.Domain.Project.Enums;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.Update;

public sealed record UpdateProjectCommand(
    Guid Id,
    string Name,
    ProjectLifecycle Lifecycle)
    : IRequest<ErrorOr<UpdateProjectResult>>;