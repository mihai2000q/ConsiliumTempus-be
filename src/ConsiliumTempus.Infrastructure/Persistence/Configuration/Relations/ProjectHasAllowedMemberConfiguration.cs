using ConsiliumTempus.Domain.Common.Relations;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration.Relations;

public sealed class ProjectHasAllowedMemberConfiguration : IEntityTypeConfiguration<ProjectHasAllowedMember>
{
    public void Configure(EntityTypeBuilder<ProjectHasAllowedMember> builder)
    {
        builder.ToTable(nameof(ProjectHasAllowedMember));

        builder.HasKey(p => new { p.ProjectAggregateId, p.AllowedMembersId });
        builder.HasOne<ProjectAggregate>()
            .WithMany()
            .HasForeignKey(p => p.ProjectAggregateId);

        builder.HasOne<UserAggregate>()
            .WithMany()
            .HasForeignKey(p => p.AllowedMembersId);

        builder.Property(p => p.ProjectAggregateId)
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value))
            .HasColumnName(nameof(ProjectId));

        builder.Property(p => p.AllowedMembersId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value))
            .HasColumnName(nameof(UserId));
    }
}