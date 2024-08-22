using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
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

    public Task<CustomFieldSetupAggregate?> GetWithWorkspace(
        CustomFieldSetupId id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.CustomFieldSetups
            .Include(cfs => cfs.Workspace)
            .SingleOrDefaultAsync(cfs => cfs.Id == id, cancellationToken);
    }
    
    public Task<CustomFieldSetupAggregate?> GetWithWorkspaceAndProjects(
        CustomFieldSetupId id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.CustomFieldSetups
            .Include(cfs => cfs.Workspace)
            .Include(cfs => cfs.Projects)
            .SingleOrDefaultAsync(cfs => cfs.Id == id, cancellationToken);
    }

    public Task<List<CustomFieldSetupAggregate>> GetList(
        WorkspaceId? workspaceId,
        ProjectId? projectId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.CustomFieldSetups
            .WhereIf(workspaceId is not null, cfs => cfs.Workspace != null && cfs.Workspace.Id == workspaceId)
            .WhereIf(projectId is not null, cfs => cfs.Projects.Any(p => p.Id == projectId))
            .OrderBy(cfs => cfs.Audit.CreatedDateTime)
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

    public async Task DeleteByProject(ProjectAggregate project, CancellationToken cancellationToken = default)
    {
        var setups = await dbContext.CustomFieldSetups
            .Where(c => c.Workspace == null)
            .Where(cfs => cfs.Projects.Contains(project))
            .ToListAsync(cancellationToken);
        dbContext.CustomFieldSetups.RemoveRange(setups);
    }

    public async Task DeleteByWorkspaceOrProjects(
        WorkspaceAggregate workspace,
        List<ProjectAggregate> projects,
        CancellationToken cancellationToken = default)
    {
        var setups = await dbContext.CustomFieldSetups
            .Where(cfs => cfs.Workspace == workspace || 
                          (cfs.Workspace == null && cfs.Projects.Any() && projects.Contains(cfs.Projects.First())))
            .ToListAsync(cancellationToken);
        dbContext.CustomFieldSetups.RemoveRange(setups);
    }
}