using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Commands.Delete;

public sealed record DeleteWorkspaceCommand(Guid Id)
    : IRequest<ErrorOr<DeleteWorkspaceResult>>;