using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.Update;

public sealed record UpdateProjectCommand(
    Guid Id,
    string Name,
    bool IsFavorite)
    : IRequest<ErrorOr<UpdateProjectResult>>;