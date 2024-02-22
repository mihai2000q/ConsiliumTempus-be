using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Create;

public sealed record CreateWorkspaceCommand(
    string Name,
    string Description) : IRequest<CreateWorkspaceResult>;