using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class WorkspaceConfiguration : IEntityTypeConfiguration<WorkspaceAggregate>
{
    public void Configure(EntityTypeBuilder<WorkspaceAggregate> builder)
    {
        builder.ToTable(nameof(WorkspaceAggregate).TruncateAggregate());

        builder.HasIndex(w => w.Id);
        builder.HasKey(w => w.Id);
        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => WorkspaceId.Create(value));

        builder.OwnsOne(w => w.Name)
            .Property(n => n.Value)
            .HasColumnName(nameof(Name))
            .HasMaxLength(PropertiesValidation.Workspace.NameMaximumLength);

        builder.OwnsOne(w => w.Description)
            .Property(d => d.Value)
            .HasColumnName(nameof(Description));

        builder.OwnsOne(w => w.IsFavorite)
            .Property(f => f.Value)
            .HasColumnName(nameof(IsFavorite));

        builder.OwnsOne(w => w.IsPersonal)
            .Property(p => p.Value)
            .HasColumnName(nameof(IsPersonal));

        builder.HasOne(w => w.Owner)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Navigation(w => w.Owner).AutoInclude();
    }
}