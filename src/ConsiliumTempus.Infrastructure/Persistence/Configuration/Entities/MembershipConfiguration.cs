using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Persistence.Configuration.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration.Entities;

internal sealed class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable(nameof(Membership));

        builder.HasOne(m => m.User)
            .WithMany(u => u.Memberships)
            .HasForeignKey(nameof(UserId))
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(m => m.User).AutoInclude();

        builder.HasOne(m => m.Workspace)
            .WithMany(w => w.Memberships)
            .HasForeignKey(nameof(WorkspaceId))
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<WorkspaceRole>(m => m.WorkspaceRole)
            .WithMany()
            .HasForeignKey(nameof(Membership.WorkspaceRole).ToIdBackingField())
            .IsRequired();
        builder.Property(nameof(Membership.WorkspaceRole).ToIdBackingField())
            .HasColumnName(nameof(Membership.WorkspaceRole).ToId());
        builder.Navigation(m => m.WorkspaceRole).AutoInclude();

        builder.OwnsAudit(m => m.Audit);

        builder.Ignore(m => m.Id);
        builder.HasKey(nameof(UserId), nameof(WorkspaceId));
    }
}