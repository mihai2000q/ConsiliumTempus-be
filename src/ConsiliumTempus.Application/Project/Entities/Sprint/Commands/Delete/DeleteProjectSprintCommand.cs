using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;

public sealed record DeleteProjectSprintCommand(Guid Id) : IRequest<ErrorOr<DeleteProjectSprintResult>>;