using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Providers;

public sealed class ProjectProvider(ConsiliumTempusDbContext dbContext) : IProjectProvider
{
    public Task<ProjectAggregate?> Get(ProjectId id, CancellationToken cancellationToken = default)
    {
        return dbContext.Projects
            .Include(p => p.AllowedMembers)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<ProjectAggregate?> GetByProjectSprint(ProjectSprintId id, CancellationToken cancellationToken = default)
    {
        var sprint = await dbContext.ProjectSprints
            .Include(ps => ps.Project)
            .ThenInclude(p => p.AllowedMembers)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);

        return sprint?.Project;
    }

    public async Task<ProjectAggregate?> GetByProjectStage(ProjectStageId id, CancellationToken cancellationToken = default)
    {
        var stage = await dbContext.Set<ProjectStage>()
            .Include(ps => ps.Sprint.Project)
            .ThenInclude(p => p.AllowedMembers)
            .SingleOrDefaultAsync(ps => ps.Id == id, cancellationToken);

        return stage?.Sprint.Project;
    }

    public async Task<ProjectAggregate?> GetByProjectTask(ProjectTaskId id, CancellationToken cancellationToken = default)
    {
        var task = await dbContext.ProjectTasks
            .Include(pt => pt.Stage.Sprint.Project)
            .ThenInclude(p => p.AllowedMembers)
            .SingleOrDefaultAsync(pt => pt.Id == id, cancellationToken);

        return task?.Stage.Sprint.Project;
    }
}