using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.UpdateOwner;

public sealed record UpdateOwnerProjectCommand(
    Guid Id,
    Guid OwnerId)
    : IRequest<ErrorOr<UpdateOwnerProjectResult>>;