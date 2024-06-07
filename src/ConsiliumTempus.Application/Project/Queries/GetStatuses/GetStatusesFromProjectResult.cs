using ConsiliumTempus.Domain.Project.Entities;

namespace ConsiliumTempus.Application.Project.Queries.GetStatuses;

public record GetStatusesFromProjectResult(
    List<ProjectStatus> Statuses,
    int TotalCount);