using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ConsiliumTempus.Infrastructure.Persistence.Interceptors;

public sealed class SolveCircularDependencyInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        SolveSingleSelectCustomFieldSetup(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        await SolveSingleSelectCustomFieldSetup(eventData.Context, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static async Task SolveSingleSelectCustomFieldSetup(DbContext? dbContext,
        CancellationToken cancellationToken = default)
    {
        if (dbContext is null) return;

        var singleSelectCustomFieldSetups = dbContext.ChangeTracker
            .Entries<SingleSelectCustomFieldSetupAggregate>()
            .Select(e => e.Entity)
            .Where(ss => ss.DefaultOption is not null)
            .ToList();

        foreach (var singleSelectCustomFieldSetup in singleSelectCustomFieldSetups)
        {
            var defaultOptionId = singleSelectCustomFieldSetup.DefaultOption!.Id;
            singleSelectCustomFieldSetup.UpdateDefaultOption(null);
            await dbContext.SaveChangesAsync(cancellationToken);

            var defaultOption = singleSelectCustomFieldSetup.Options.Single(o => o.Id == defaultOptionId);
            singleSelectCustomFieldSetup.UpdateDefaultOption(defaultOption);
        }
    }
}