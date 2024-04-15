using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable(nameof(Membership));

        builder.Ignore(m => m.Id);

        builder.HasOne(m => m.User)
            .WithMany(u => u.Memberships)
            .HasForeignKey(nameof(UserId));

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

        builder.HasKey(nameof(UserId), nameof(WorkspaceId));
    }
}