using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class WorkspaceConfiguration : IEntityTypeConfiguration<WorkspaceAggregate>
{
    public void Configure(EntityTypeBuilder<WorkspaceAggregate> builder)
    {
        ConfigureWorkspacesTable(builder);
    }

    private static void ConfigureWorkspacesTable(EntityTypeBuilder<WorkspaceAggregate> builder)
    {
        builder.ToTable("Workspace");

        builder.HasIndex(w => w.Id);
        builder.HasKey(w => w.Id);
        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => WorkspaceId.Create(value));

        builder.Property(w => w.Name)
            .HasMaxLength(PropertiesValidation.Workspace.NameMaximumLength);

        builder.Property(w => w.Description)
            .HasMaxLength(PropertiesValidation.Workspace.DescriptionMaximumLength);
    }
}