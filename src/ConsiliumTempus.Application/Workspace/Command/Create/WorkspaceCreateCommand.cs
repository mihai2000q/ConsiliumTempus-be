using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Command.Create;

public record WorkspaceCreateCommand(
    string Name,
    string Description) : IRequest<ErrorOr<WorkspaceCreateResult>>;