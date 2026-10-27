using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ConsiliumTempus.Infrastructure.Persistence.Conventions;

/// <summary>
/// The application generates all the primary keys (the identifiers are created in the domain),
/// so the database never generates any of them.
/// </summary>
internal sealed class NeverGenerateKeysConvention : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(
        IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context)
    {
        var keyProperties = modelBuilder.Metadata
            .GetEntityTypes()
            .SelectMany(e => e.GetDeclaredProperties())
            .Where(p => p.IsPrimaryKey());

        // The source has to outweigh the one of the conventions that generate the keys,
        // otherwise they would win (and the SQL Server provider would make the integer keys IDENTITY)
        foreach (var property in keyProperties)
        {
            property.Builder.ValueGenerated(ValueGenerated.Never, fromDataAnnotation: true);
            property.Builder.HasValueGenerationStrategy(SqlServerValueGenerationStrategy.None, fromDataAnnotation: true);
        }
    }
}
