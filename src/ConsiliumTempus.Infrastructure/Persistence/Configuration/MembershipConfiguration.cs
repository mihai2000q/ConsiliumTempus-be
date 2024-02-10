using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
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

        builder.HasOne(m => m.WorkspaceRole)
            .WithMany();
        
        builder.HasKey(nameof(UserId), nameof(WorkspaceId));
    }
}