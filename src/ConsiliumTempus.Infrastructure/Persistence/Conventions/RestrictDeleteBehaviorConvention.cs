using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ConsiliumTempus.Infrastructure.Persistence.Conventions;

/// <summary>
/// By default, EF cascades the delete of required relationships, which can delete data by accident.
/// This makes every relationship restrictive, unless its delete behavior is configured explicitly
/// (it has to be said whenever a relationship should cascade or set to null)
/// </summary>
internal sealed class RestrictDeleteBehaviorConvention : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(
        IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context)
    {
        var foreignKeys = modelBuilder.Metadata
            .GetEntityTypes()
            .SelectMany(e => e.GetDeclaredForeignKeys())
            .Where(fk => !fk.IsOwnership)
            .Where(fk => fk.GetDeleteBehaviorConfigurationSource()
                is not (ConfigurationSource.Explicit or ConfigurationSource.DataAnnotation));

        foreach (var foreignKey in foreignKeys)
            foreignKey.Builder.OnDelete(IsInheritanceLink(foreignKey) ? DeleteBehavior.Cascade : DeleteBehavior.Restrict);
    }

    /// <summary>
    /// The row of a type that is mapped to its own table (table per type) cannot exist without the row of its base type,
    /// so the delete of the base has to cascade to it (EF creates this foreign key itself, and it cannot be configured)
    /// </summary>
    private static bool IsInheritanceLink(IConventionForeignKey foreignKey)
    {
        return foreignKey.DeclaringEntityType.BaseType == foreignKey.PrincipalEntityType &&
               foreignKey.Properties.SequenceEqual(foreignKey.DeclaringEntityType.FindPrimaryKey()?.Properties ?? []);
    }
}
