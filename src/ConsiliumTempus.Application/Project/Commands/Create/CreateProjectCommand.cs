using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.Create;

public sealed record CreateProjectCommand(
    Guid WorkspaceId,
    string Name,
    bool IsPrivate)
    : IRequest<ErrorOr<CreateProjectResult>>;