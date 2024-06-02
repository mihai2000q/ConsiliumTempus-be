using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IProjectRepository
{
    Task<ProjectAggregate?> Get(ProjectId projectId, CancellationToken cancellationToken = default);

    Task<ProjectAggregate?> GetWithWorkspace(ProjectId id, CancellationToken cancellationToken = default);

    Task<ProjectAggregate?> GetWithStagesAndWorkspace(ProjectId id, CancellationToken cancellationToken = default);

    Task<List<ProjectAggregate>> GetListByUser(
        UserId userId,
        WorkspaceId? workspaceId,
        PaginationInfo? paginationInfo,
        IReadOnlyList<IOrder<ProjectAggregate>> orders,
        IEnumerable<IFilter<ProjectAggregate>> filters,
        CancellationToken cancellationToken = default);

    Task<int> GetListByUserCount(
        UserId userId,
        WorkspaceId? workspaceId,
        IEnumerable<IFilter<ProjectAggregate>> filters,
        CancellationToken cancellationToken = default);

    Task<List<ProjectAggregate>> GetFavorites(
        UserId userId,
        CancellationToken cancellationToken = default);

    Task Add(ProjectAggregate project, CancellationToken cancellationToken = default);

    void Remove(ProjectAggregate project);
}