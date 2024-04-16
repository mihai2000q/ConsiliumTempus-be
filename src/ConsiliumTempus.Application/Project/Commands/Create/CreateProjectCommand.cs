using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.Create;

public sealed record CreateProjectCommand(
    Guid WorkspaceId,
    string Name,
    string Description,
    bool IsPrivate)
    : IRequest<ErrorOr<CreateProjectResult>>;