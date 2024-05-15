using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Entities.Stage.Queries.GetCollection;

public sealed record GetCollectionProjectStageQuery(
    Guid ProjectSprintId)
    : IRequest<ErrorOr<GetCollectionProjectStageResult>>;