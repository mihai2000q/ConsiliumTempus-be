using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.Entities;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IWorkspaceRepository
{
    Task<WorkspaceAggregate?> Get(WorkspaceId id, CancellationToken cancellationToken = default);

    Task<WorkspaceAggregate?> GetWithCollaborators(
        WorkspaceId id,
        CancellationToken cancellationToken = default);

    Task<WorkspaceAggregate?> GetWithInvitations(
        WorkspaceId id,
        CancellationToken cancellationToken = default);

    Task<WorkspaceAggregate?> GetWithCollaboratorsAndInvitations(
        WorkspaceId id,
        CancellationToken cancellationToken = default);

    Task<List<WorkspaceAggregate>> GetListByUser(
        UserAggregate user,
        PaginationInfo? paginationInfo,
        IReadOnlyList<IOrder<WorkspaceAggregate>> orders,
        IEnumerable<IFilter<WorkspaceAggregate>> filters,
        CancellationToken cancellationToken = default);

    Task<int> GetListByUserCount(
        UserAggregate user,
        IEnumerable<IFilter<WorkspaceAggregate>> filters,
        CancellationToken cancellationToken = default);

    Task<List<WorkspaceAggregate>> GetListByUserWithCollaborators(UserAggregate user,
        CancellationToken cancellationToken = default);

    Task<List<UserAggregate>> GetCollaborators(
        WorkspaceId id,
        string? searchValue,
        CancellationToken cancellationToken = default);

    Task<List<WorkspaceInvitation>> GetInvitations(
        UserAggregate? user,
        bool? isSender,
        WorkspaceId? workspaceId,
        PaginationInfo? paginationInfo,
        CancellationToken cancellationToken = default);

    Task<int> GetInvitationsCount(
        UserAggregate? user,
        bool? isSender,
        WorkspaceId? workspaceId,
        CancellationToken cancellationToken = default);

    Task Add(WorkspaceAggregate workspace, CancellationToken cancellationToken = default);

    void Remove(WorkspaceAggregate workspace);
}