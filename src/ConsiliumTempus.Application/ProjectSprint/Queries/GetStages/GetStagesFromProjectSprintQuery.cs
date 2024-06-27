using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.GetStages;

public record GetStagesFromProjectSprintQuery(Guid Id) : IRequest<ErrorOr<GetStagesFromProjectSprintResult>>;