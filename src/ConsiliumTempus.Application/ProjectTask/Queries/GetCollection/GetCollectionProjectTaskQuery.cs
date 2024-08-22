using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;

public sealed record GetCollectionProjectTaskQuery(
    Guid ProjectStageId,
    List<string> Search,
    List<string> OrderBy,
    int? CurrentPage,
    int? PageSize)
    : IRequest<ErrorOr<GetCollectionProjectTaskResult>>;