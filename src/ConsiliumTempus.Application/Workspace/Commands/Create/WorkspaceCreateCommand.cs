using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Create;

public record WorkspaceCreateCommand(
    string Name,
    string Description,
    string Token) : IRequest<ErrorOr<WorkspaceCreateResult>>;