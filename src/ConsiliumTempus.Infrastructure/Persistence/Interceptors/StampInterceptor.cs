using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Configuration.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Stamps the <see cref="Audit"/> and the <see cref="Authorship"/> of the entities that are saved:
/// the current user and the date and time of the changes.
/// They are created for the added entities (unless they already have one), and updated for the modified ones
/// (and the audit also for the ones that are marked as updated, see <see cref="ConsiliumTempusDbContext.MarkAsUpdated"/>).
/// It has to go after the <see cref="PublishDomainEventsInterceptor"/>, as the domain events add and modify entities too.
/// </summary>
public sealed class StampInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        Stamp(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        await Stamp(eventData.Context, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task Stamp(DbContext? dbContext, CancellationToken cancellationToken = default)
    {
        if (dbContext is null) return;

        var markedAsUpdated = (dbContext as ConsiliumTempusDbContext)?.TakeMarkedAsUpdated() ?? [];
        var audited = dbContext.ChangeTracker
            .Entries<IAudit>()
            .Where(entry => entry.State == EntityState.Added || IsUpdated(entry) || markedAsUpdated.Contains(entry.Entity))
            .ToList();
        var authored = dbContext.ChangeTracker
            .Entries<IAuthored>()
            .Where(entry => entry.State == EntityState.Added || IsUpdated(entry))
            .ToList();
        if (audited.Count == 0 && authored.Count == 0) return;

        var now = DateTime.UtcNow;
        // the user is not needed to update an authorship, as its author never changes
        var user = audited.Count > 0 || authored.Any(entry => entry.State == EntityState.Added)
            ? await GetCurrentUser(dbContext, cancellationToken)
            : null;

        foreach (var entry in audited)
        {
            var audit = entry.Reference(nameof(IAudit.Audit));
            if (entry.State == EntityState.Added)
            {
                // an audit that is already there is kept (i.e., the one of the data that is imported)
                audit.CurrentValue ??= Audit.Create(user, now);
            }
            else
            {
                audit.CurrentValue = entry.Entity.Audit.Update(user, now);
            }
        }

        foreach (var entry in authored)
        {
            var authorship = entry.Reference(nameof(IAuthored.Authorship));
            if (entry.State == EntityState.Added)
            {
                authorship.CurrentValue ??= Authorship.Create(user, now);
            }
            else
            {
                authorship.CurrentValue = entry.Entity.Authorship.Update(now);
            }
        }

        dbContext.ChangeTracker.DetectChanges();
    }

    /// <summary>
    /// An entity is updated when its own properties or its owned types (i.e., its name) change.
    /// The audit and the authorship are not a change of the entity, and neither is what is excluded from the audit
    /// (see <see cref="AuditExtensions.ExcludeFromAudit{TProperty}"/>), so they do not count.
    /// </summary>
    private static bool IsUpdated(EntityEntry entry)
    {
        if (entry.State == EntityState.Modified &&
            entry.Properties.Any(property => property.IsModified && !property.Metadata.IsExcludedFromAudit()))
            return true;

        return entry.References
            .Where(reference => reference.Metadata is INavigation { ForeignKey.IsOwnership: true } &&
                                reference.Metadata.Name != nameof(IAudit.Audit) &&
                                reference.Metadata.Name != nameof(IAuthored.Authorship) &&
                                !reference.Metadata.IsExcludedFromAudit())
            .Any(reference => reference.TargetEntry is { State: not EntityState.Unchanged });
    }

    /// <summary>
    /// Nobody is logged in when a user registers, so the user that is registering is the one who does it
    /// (i.e., the personal workspace that is created for them). It is only when exactly one user is added,
    /// so that nothing else can be stamped with a user that did not do it.
    /// </summary>
    private static UserAggregate? GetRegisteringUser(DbContext dbContext)
    {
        var addedUsers = dbContext.ChangeTracker
            .Entries<UserAggregate>()
            .Where(entry => entry.State == EntityState.Added)
            .Select(entry => entry.Entity)
            .Take(2)
            .ToList();

        return addedUsers.Count == 1 ? addedUsers[0] : null;
    }

    private async Task<UserAggregate?> GetCurrentUser(DbContext dbContext, CancellationToken cancellationToken)
    {
        var subUserId = httpContextAccessor.HttpContext?.User.Claims
            .SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (subUserId is null) return GetRegisteringUser(dbContext);
        if (!Guid.TryParse(subUserId, out var guidUserId)) return null;

        var userId = UserId.Create(guidUserId);
        var user = dbContext.Set<UserAggregate>().Local.FirstOrDefault(u => u.Id == userId) ??
                   await dbContext.Set<UserAggregate>().FindAsync([userId], cancellationToken);

        // the current user that is being deleted cannot be stamped
        return user is not null && dbContext.Entry(user).State == EntityState.Deleted ? null : user;
    }
}