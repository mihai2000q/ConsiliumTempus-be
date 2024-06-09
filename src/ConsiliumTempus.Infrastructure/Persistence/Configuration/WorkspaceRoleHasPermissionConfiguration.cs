using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class WorkspaceRoleHasPermissionConfiguration : IEntityTypeConfiguration<WorkspaceRoleHasPermission>
{
    public void Configure(EntityTypeBuilder<WorkspaceRoleHasPermission> builder)
    {
        builder.ToTable(nameof(WorkspaceRoleHasPermission));

        builder.Ignore(w => w.Id);
        builder.HasKey(w => new { w.WorkspaceRoleId, w.PermissionId });

        builder.HasOne<WorkspaceRole>()
            .WithMany()
            .HasForeignKey(w => w.WorkspaceRoleId);

        builder.HasOne<Permission>()
            .WithMany()
            .HasForeignKey(w => w.PermissionId);

        foreach (var roleHasPermission in WorkspaceRoleHasPermission.DefaultData)
        {
            roleHasPermission.Value.ForEach(p => builder.HasData(Create(roleHasPermission.Key, p)));
        }
    }

    private static WorkspaceRoleHasPermission Create(WorkspaceRole workspaceRole, Permissions permission)
    {
        return WorkspaceRoleHasPermission.Create(workspaceRole.Id, (int)permission + 1);
    }
}