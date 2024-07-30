using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable(nameof(Membership));

        builder.HasOne(m => m.User)
            .WithMany(u => u.Memberships)
            .HasForeignKey(nameof(UserId));
        builder.Navigation(m => m.User).AutoInclude();

        builder.HasOne(m => m.Workspace)
            .WithMany(w => w.Memberships)
            .HasForeignKey(nameof(WorkspaceId));

        builder.HasOne<WorkspaceRole>()
            .WithMany()
            .HasForeignKey(nameof(Membership.WorkspaceRole).ToIdBackingField())
            .IsRequired();
        builder.Property(nameof(Membership.WorkspaceRole).ToIdBackingField())
            .HasColumnName(nameof(Membership.WorkspaceRole).ToId());
        builder.Ignore(m => m.WorkspaceRole);

        builder.Ignore(m => m.Id);
        builder.HasKey(nameof(UserId), nameof(WorkspaceId));
    }
}