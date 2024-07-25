using ConsiliumTempus.Domain.Project.Entities;

namespace ConsiliumTempus.Application.Project.Queries.GetStatuses;

public sealed record GetStatusesFromProjectResult(
    List<ProjectStatus> Statuses,
    int TotalCount);