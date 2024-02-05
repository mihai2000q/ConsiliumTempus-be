using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.WorkspaceAggregate;
using ConsiliumTempus.Domain.WorkspaceAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        ConfigureWorkspacesTable(builder);
    }

    private static void ConfigureWorkspacesTable(EntityTypeBuilder<Workspace> builder)
    {
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