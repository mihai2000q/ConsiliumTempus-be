using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;

public sealed record GetCollectionProjectTaskQuery(
    Guid ProjectStageId,
    string[]? Search)
    : IRequest<ErrorOr<GetCollectionProjectTaskResult>>;