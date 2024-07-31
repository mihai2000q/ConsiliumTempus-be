using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Providers;

public sealed class WorkspaceProvider(ConsiliumTempusDbContext dbContext) : IWorkspaceProvider
{
    public async Task<WorkspaceAggregate?> Get(WorkspaceId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces.FindAsync([id], cancellationToken);
    }

    public async Task<WorkspaceAggregate?> GetWithMemberships(
        WorkspaceId id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Workspaces
            .Include(w => w.Memberships)
            .SingleOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<WorkspaceAggregate?> GetByProject(ProjectId id, CancellationToken cancellationToken = default)
    {
        var project = await dbContext.Projects
            .Include(p => p.Workspace)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

        return project?.Workspace;
    }

    public async Task<WorkspaceAggregate?> GetByProjectWithMemberships(ProjectId id, 
        CancellationToken cancellationToken = default)
    {
        var project = await dbContext.Projects
            .Include(p => p.Workspace.Memberships)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

        return project?.Workspace;
    }

    public async Task<WorkspaceAggregate?> GetByProjectSprint(
        ProjectSprintId id,
        CancellationToken cancellationToken = default)
    {
        var projectSprint = await dbContext.ProjectSprints
            .Include(ps => ps.Project.Workspace)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);

        return projectSprint?.Project.Workspace;
    }

    public async Task<WorkspaceAggregate?> GetByProjectStage(
        ProjectStageId id,
        CancellationToken cancellationToken = default)
    {
        var projectStage = await dbContext.Set<ProjectStage>()
            .Include(ps => ps.Sprint.Project.Workspace)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);

        return projectStage?.Sprint.Project.Workspace;
    }

    public async Task<WorkspaceAggregate?> GetByProjectTask(
        ProjectTaskId id,
        CancellationToken cancellationToken = default)
    {
        var projectTask = await dbContext.ProjectTasks
            .Include(pt => pt.Stage.Sprint.Project.Workspace)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);

        return projectTask?.Stage.Sprint.Project.Workspace;
    }
}