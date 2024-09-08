using ConsiliumTempus.Domain.Project.Enums;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.AddStatus;

public sealed record AddStatusToProjectCommand(
    Guid Id,
    string Title,
    ProjectStatusType Status,
    string Description)
    : IRequest<ErrorOr<AddStatusToProjectResult>>;