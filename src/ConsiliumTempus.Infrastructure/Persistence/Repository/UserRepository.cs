using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.Entities;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

internal sealed class UserRepository(ConsiliumTempusDbContext dbContext) : IUserRepository, IUserProvider
{
    public async Task<UserAggregate?> Get(UserId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.FindAsync([id], cancellationToken);
    }

    public async Task<UserAggregate?> GetByEmail(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.SingleOrDefaultAsync(
            u => u.Credentials.Email == email,
            cancellationToken);
    }

    public async Task Add(UserAggregate user, CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
    }

    public void Remove(UserAggregate user)
    {
        dbContext.Users.Remove(user);
    }

    // ReSharper disable EntityFramework.NPlusOne.IncompleteDataQuery - always included
    // ReSharper disable EntityFramework.NPlusOne.IncompleteDataUsage
    public async Task ClearReferences(UserAggregate user, CancellationToken cancellationToken = default)
    {
        // Clear the user references on tasks and the audit
        var tasks = await dbContext.ProjectTasks
            .Include(t => t.Comments)
            .Where(t => t.Assignee == user || t.Reviewer == user ||
                        t.Comments.Any(c => c.CreatedBy == user) ||
                        t.Audit.UpdatedBy == user || t.Audit.CreatedBy == user)
            .ToListAsync(cancellationToken);
        tasks.ForEach(task =>
        {
            task.ClearUser(user);
            if (task.Audit.UpdatedBy == user || task.Audit.CreatedBy == user)
                dbContext.Entry(task).Reference(nameof(IAudit.Audit)).CurrentValue = task.Audit.ClearUser(user);
        });

        // The above change must be stamped on audit, but those below can skip it
        await ClearAuditUser(dbContext.Set<CustomFieldSetupAggregate>(), user, cancellationToken);
        await ClearAuditUser(dbContext.Set<ProjectStatus>(), user, cancellationToken);
        await ClearAuditUser(dbContext.ProjectSprints, user, cancellationToken);
        await ClearAuditUser(dbContext.Set<ProjectStage>(), user, cancellationToken);
        await ClearAuditUser(dbContext.Projects, user, cancellationToken);
        await ClearAuditUser(dbContext.Workspaces, user, cancellationToken);
        await ClearAuditUser(dbContext.Set<Membership>(), user, cancellationToken);
    }

    public async Task RemoveWorkspaceInvitationsByUser(UserAggregate user,
        CancellationToken cancellationToken = default)
    {
        var invitations = await dbContext.Set<WorkspaceInvitation>()
            .Where(wi => wi.Sender == user || wi.Collaborator == user)
            .ToListAsync(cancellationToken);

        dbContext.Set<WorkspaceInvitation>().RemoveRange(invitations);
    }

    private static async Task ClearAuditUser<TAudit>(
        IQueryable<TAudit> entities,
        UserAggregate user,
        CancellationToken cancellationToken)
        where TAudit : class, IAudit
    {
        var createdById = nameof(Audit.CreatedBy).ToId();
        var updatedById = nameof(Audit.UpdatedBy).ToId();

        await entities
            .Where(e => EF.Property<UserId?>(e.Audit, createdById) == user.Id)
            .ExecuteUpdateAsync(
                s => s.SetProperty(e =>
                    EF.Property<UserId?>(e.Audit, createdById), (UserId?)null),
                cancellationToken);
        await entities
            .Where(e => EF.Property<UserId?>(e.Audit, updatedById) == user.Id)
            .ExecuteUpdateAsync(
                s => s.SetProperty(e =>
                    EF.Property<UserId?>(e.Audit, updatedById), (UserId?)null),
                cancellationToken);
    }
}