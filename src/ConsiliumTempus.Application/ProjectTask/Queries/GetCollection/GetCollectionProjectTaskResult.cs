using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;

public sealed record GetCollectionProjectTaskResult(
    List<ProjectTaskAggregate> Tasks,
    int TotalCount);