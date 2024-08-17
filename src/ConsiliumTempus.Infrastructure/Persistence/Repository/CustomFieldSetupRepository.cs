using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class CustomFieldSetupRepository(ConsiliumTempusDbContext dbContext) : ICustomFieldSetupRepository
{
    public async Task<CustomFieldSetupAggregate?> Get(CustomFieldSetupId id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.CustomFieldSetups.FindAsync([id], cancellationToken);
    }
    
    public Task<CustomFieldSetupAggregate?> GetWithWorkspaceAndProject(
        CustomFieldSetupId id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.CustomFieldSetups
            .Include(cfs => cfs.Workspace)
            .Include(cfs => cfs.Project)
            .SingleOrDefaultAsync(cfs => cfs.Id == id, cancellationToken);
    }

    public Task<List<CustomFieldSetupAggregate>> GetList(
        WorkspaceId? workspaceId,
        ProjectId? projectId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.CustomFieldSetups
            .WhereIf(workspaceId is not null, cfs => cfs.Workspace != null && cfs.Workspace.Id == workspaceId)
            .WhereIf(projectId is not null, cfs => cfs.Project != null && cfs.Project.Id == projectId)
            .ToListAsync(cancellationToken);
    }

    public async Task Add(CustomFieldSetupAggregate customFieldSetup, CancellationToken cancellationToken = default)
    {
        await dbContext.CustomFieldSetups.AddAsync(customFieldSetup, cancellationToken);
    }

    public void Remove(CustomFieldSetupAggregate customFieldSetup)
    {
        dbContext.CustomFieldSetups.Remove(customFieldSetup);
    }

    public async Task DeleteByProject(ProjectId projectId, CancellationToken cancellationToken = default)
    {
        var setups = await dbContext.CustomFieldSetups
            .Where(cfs => cfs.Workspace == null && cfs.Project != null && cfs.Project.Id == projectId)
            .ToListAsync(cancellationToken);
        dbContext.CustomFieldSetups.RemoveRange(setups);
    }

    public async Task DeleteByWorkspaceOrProjects(
        WorkspaceId workspaceId,
        List<ProjectAggregate> projects,
        CancellationToken cancellationToken = default)
    {
        var setups = await dbContext.CustomFieldSetups
            .Where(cfs => 
                (cfs.Workspace != null && cfs.Workspace.Id == workspaceId) ||
                (cfs.Project != null && projects.Contains(cfs.Project)))
            .ToListAsync(cancellationToken);
        dbContext.CustomFieldSetups.RemoveRange(setups);
    }
}