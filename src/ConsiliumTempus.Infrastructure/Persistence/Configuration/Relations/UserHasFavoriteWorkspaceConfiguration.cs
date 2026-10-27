using ConsiliumTempus.Domain.Common.Relations;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration.Relations;

internal sealed class UserHasFavoriteWorkspaceConfiguration : IEntityTypeConfiguration<UserHasFavoriteWorkspace>
{
    public void Configure(EntityTypeBuilder<UserHasFavoriteWorkspace> builder)
    {
        builder.ToTable(nameof(UserHasFavoriteWorkspace));

        builder.HasKey(u => new { WorkspaceId = u.WorkspaceAggregateId, UserId = u.FavoritesId });
        builder.HasOne<WorkspaceAggregate>()
            .WithMany()
            .HasForeignKey(u => u.WorkspaceAggregateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UserAggregate>()
            .WithMany()
            .HasForeignKey(u => u.FavoritesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(u => u.WorkspaceAggregateId)
            .HasConversion(
                id => id.Value,
                value => WorkspaceId.Create(value))
            .HasColumnName(nameof(WorkspaceId));

        builder.Property(u => u.FavoritesId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value))
            .HasColumnName(nameof(UserId));
    }
}