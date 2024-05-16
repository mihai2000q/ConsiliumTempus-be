using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Commands.Create;

public sealed record CreateProjectTaskCommand(
    Guid ProjectStageId,
    string Name,
    bool OnTop)
    : IRequest<ErrorOr<CreateProjectTaskResult>>;