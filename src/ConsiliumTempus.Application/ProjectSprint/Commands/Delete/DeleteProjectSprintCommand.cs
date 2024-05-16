using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.Delete;

public sealed record DeleteProjectSprintCommand(Guid Id) : IRequest<ErrorOr<DeleteProjectSprintResult>>;