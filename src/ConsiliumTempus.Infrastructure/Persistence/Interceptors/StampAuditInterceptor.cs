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
/// Stamps the <see cref="Audit"/> of the entities that are saved: the current user and the date and time of the changes.
/// It is created for the added entities (unless they already have one), and updated for the modified ones
/// (and for the ones that are marked as updated, see <see cref="ConsiliumTempusDbContext.MarkAsUpdated"/>).
/// It has to go after the <see cref="PublishDomainEventsInterceptor"/>, as the domain events add and modify entities too.
/// </summary>
public sealed class StampAuditInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        StampAudits(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        await StampAudits(eventData.Context, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task StampAudits(DbContext? dbContext, CancellationToken cancellationToken = default)
    {
        if (dbContext is null) return;

        var markedAsUpdated = (dbContext as ConsiliumTempusDbContext)?.TakeMarkedAsUpdated() ?? [];
        var entries = dbContext.ChangeTracker
            .Entries<IAudit>()
            .Where(entry => entry.State == EntityState.Added || IsUpdated(entry) || markedAsUpdated.Contains(entry.Entity))
            .ToList();
        if (entries.Count == 0) return;

        var user = await GetCurrentUser(dbContext, cancellationToken);
        var now = DateTime.UtcNow;

        foreach (var entry in entries)
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

        dbContext.ChangeTracker.DetectChanges();
    }

    /// <summary>
    /// An entity is updated when its own properties or its owned types (i.e., its name) change.
    /// The audit is not a change of the entity, and neither is what is excluded from it
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
                                !reference.Metadata.IsExcludedFromAudit())
            .Any(reference => reference.TargetEntry is { State: not EntityState.Unchanged });
    }

    private async Task<UserAggregate?> GetCurrentUser(DbContext dbContext, CancellationToken cancellationToken)
    {
        var subUserId = httpContextAccessor.HttpContext?.User.Claims
            .SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (subUserId is null || !Guid.TryParse(subUserId, out var guidUserId)) return null;

        var userId = UserId.Create(guidUserId);
        var user = dbContext.Set<UserAggregate>().Local.FirstOrDefault(u => u.Id == userId) ??
                   await dbContext.Set<UserAggregate>().FindAsync([userId], cancellationToken);

        // the current user that is being deleted cannot be stamped
        return user is not null && dbContext.Entry(user).State == EntityState.Deleted ? null : user;
    }
}