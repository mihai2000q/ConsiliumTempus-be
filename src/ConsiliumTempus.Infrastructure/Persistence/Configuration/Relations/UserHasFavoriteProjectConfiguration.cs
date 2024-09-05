using ConsiliumTempus.Domain.Common.Relations;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration.Relations;

public sealed class UserHasFavoriteProjectConfiguration : IEntityTypeConfiguration<UserHasFavoriteProject>
{
    public void Configure(EntityTypeBuilder<UserHasFavoriteProject> builder)
    {
        builder.ToTable(nameof(UserHasFavoriteProject));

        builder.HasKey(u => new { ProjectId = u.ProjectAggregateId, UserId = u.FavoritesId });
        builder.HasOne<ProjectAggregate>()
            .WithMany()
            .HasForeignKey(u => u.ProjectAggregateId);

        builder.HasOne<UserAggregate>()
            .WithMany()
            .HasForeignKey(u => u.FavoritesId);

        builder.Property(u => u.ProjectAggregateId)
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value))
            .HasColumnName(nameof(ProjectId));

        builder.Property(u => u.FavoritesId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value))
            .HasColumnName(nameof(UserId));
    }
}