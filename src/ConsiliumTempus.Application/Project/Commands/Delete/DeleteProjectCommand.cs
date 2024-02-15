using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.Delete;

public sealed record DeleteProjectCommand(Guid Id) 
    : IRequest<ErrorOr<DeleteProjectResult>>;