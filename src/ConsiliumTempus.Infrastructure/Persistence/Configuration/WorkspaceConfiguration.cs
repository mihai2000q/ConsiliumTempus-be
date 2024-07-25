using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.Entities;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class WorkspaceConfiguration : IEntityTypeConfiguration<WorkspaceAggregate>
{
    public void Configure(EntityTypeBuilder<WorkspaceAggregate> builder)
    {
        builder.ToTable(nameof(WorkspaceAggregate).TruncateAggregate());

        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id)
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

        builder.OwnsOne(w => w.IsPersonal)
            .Property(p => p.Value)
            .HasColumnName(nameof(IsPersonal));

        builder.HasOne(w => w.Owner)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Navigation(w => w.Owner).AutoInclude();

        builder.HasMany(w => w.Favorites)
            .WithMany()
            .UsingEntity(b =>
            {
                b.ToTable("UserHasFavoriteWorkspace");

                b.Property(nameof(WorkspaceAggregate).ToId())
                    .HasColumnName(nameof(WorkspaceId));
            });

        builder.HasMany(w => w.Invitations)
            .WithOne(i => i.Workspace);
    }
}

public sealed class WorkspaceInvitationConfiguration : IEntityTypeConfiguration<WorkspaceInvitation>
{
    public void Configure(EntityTypeBuilder<WorkspaceInvitation> builder)
    {
        builder.ToTable(nameof(WorkspaceInvitation));

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => WorkspaceInvitationId.Create(value));

        builder.HasOne(i => i.Sender)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Navigation(i => i.Sender).AutoInclude();

        builder.HasOne(i => i.Collaborator)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Navigation(i => i.Collaborator).AutoInclude();

        builder.HasOne(i => i.Workspace)
            .WithMany(w => w.Invitations);
    }
}