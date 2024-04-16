using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class WorkspaceRoleConfiguration : IEntityTypeConfiguration<WorkspaceRole>
{
    public void Configure(EntityTypeBuilder<WorkspaceRole> builder)
    {
        builder.ToTable(nameof(WorkspaceRole));

        builder.HasKey(wr => wr.Id);
        builder.Property(wr => wr.Id)
            .HasColumnOrder(0);

        builder.Property(wr => wr.Name)
            .HasColumnOrder(1);

        builder.HasMany(wr => wr.Permissions)
            .WithMany()
            .UsingEntity<WorkspaceRoleHasPermission>();

        builder.HasData(WorkspaceRole.GetValues());
    }
}