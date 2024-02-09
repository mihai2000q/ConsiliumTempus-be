using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable(nameof(Membership));

        builder.HasKey(m => m.Id);
        
        builder.HasOne<UserAggregate>(m => m.User)
            .WithMany(u => u.Memberships);

        builder.HasOne<WorkspaceAggregate>(m => m.Workspace)
            .WithMany(w => w.Memberships);

        builder.HasOne<WorkspaceRole>(m => m.WorkspaceRole)
            .WithMany();
    }
}