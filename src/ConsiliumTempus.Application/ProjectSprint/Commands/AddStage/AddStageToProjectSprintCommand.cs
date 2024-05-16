using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;

public sealed record AddStageToProjectSprintCommand(
    Guid Id,
    string Name,
    bool OnTop)
    : IRequest<ErrorOr<AddStageToProjectSprintResult>>;