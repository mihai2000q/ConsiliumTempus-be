using ConsiliumTempus.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public class WorkspaceRoleConfiguration : IEntityTypeConfiguration<WorkspaceRole>
{
    public void Configure(EntityTypeBuilder<WorkspaceRole> builder)
    {
        builder.ToTable(nameof(WorkspaceRole));
        
        builder.HasKey(wr => wr.Id);
        builder.Property(wr => wr.Id)
            .HasColumnOrder(0);

        builder.Property(wr => wr.Name)
            .HasColumnOrder(1);

        builder.Ignore(wr => wr.Permissions); // resolved in WorkspaceRoleHasPermissionConfiguration

        builder.HasData(WorkspaceRole.GetValues());
    }
}