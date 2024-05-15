using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Create;

public sealed record CreateWorkspaceCommand(
    string Name)
    : IRequest<ErrorOr<CreateWorkspaceResult>>;