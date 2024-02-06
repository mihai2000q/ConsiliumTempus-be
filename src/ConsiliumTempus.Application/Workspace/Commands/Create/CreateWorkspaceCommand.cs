using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Create;

public record CreateWorkspaceCommand(
    string Name,
    string Description,
    string Token) : IRequest<ErrorOr<CreateWorkspaceResult>>;