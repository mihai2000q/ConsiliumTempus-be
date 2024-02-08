using ConsiliumTempus.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public class WorkspaceRoleConfiguration : IEntityTypeConfiguration<WorkspaceRole>
{
    public void Configure(EntityTypeBuilder<WorkspaceRole> builder)
    {
        builder.ToTable("WorkspaceRole");
        
        builder.HasKey(wr => wr.Id);

        builder.Property(wr => wr.Name);

        builder.Ignore(wr => wr.Permissions); // resolved in WorkspaceRoleHasPermissionConfiguration

        builder.HasData(WorkspaceRole.GetValues());
    }
}