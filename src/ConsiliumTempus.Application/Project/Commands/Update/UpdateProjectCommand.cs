using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.Update;

public sealed record UpdateProjectCommand(
    Guid Id,
    string Name,
    string Lifecycle,
    bool IsFavorite)
    : IRequest<ErrorOr<UpdateProjectResult>>;