using ConsiliumTempus.Domain.Common.Relations;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public class UserToWorkspaceConfiguration : IEntityTypeConfiguration<UserToWorkspace>
{
    public void Configure(EntityTypeBuilder<UserToWorkspace> builder)
    {
        builder.ToTable(nameof(UserToWorkspace));
        
        builder.HasOne<UserAggregate>(u => u.User)
            .WithMany()
            .HasForeignKey(nameof(UserId))
            .HasPrincipalKey(u => u.Id);

        builder.HasOne<WorkspaceAggregate>(u => u.Workspace)
            .WithMany()
            .HasForeignKey(nameof(WorkspaceId))
            .HasPrincipalKey(w => w.Id);

        builder.HasKey(nameof(UserId), nameof(WorkspaceId));
        builder.HasIndex(nameof(UserId), nameof(WorkspaceId));
    }
}