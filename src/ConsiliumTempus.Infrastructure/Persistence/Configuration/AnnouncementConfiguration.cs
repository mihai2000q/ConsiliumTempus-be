using ConsiliumTempus.Domain.Announcement;
using ConsiliumTempus.Domain.Announcement.ValueObjects;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class AnnouncementConfiguration : IEntityTypeConfiguration<AnnouncementAggregate>
{
    public void Configure(EntityTypeBuilder<AnnouncementAggregate> builder)
    {
        builder.ToTable(nameof(AnnouncementAggregate).TruncateAggregate());

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => AnnouncementId.Create(value));

        builder.OwnsOne(a => a.Title)
            .Property(n => n.Value)
            .HasColumnName(nameof(Title))
            .HasMaxLength(PropertiesValidation.Announcement.TitleMaximumLength);

        builder.OwnsOne(a => a.Description)
            .Property(n => n.Value)
            .HasColumnName(nameof(Description));

        builder.HasOne(a => a.Audit)
            .WithMany();
        builder.Navigation(s => s.Audit).AutoInclude();

        builder.HasOne(a => a.Project)
            .WithMany();

        builder.HasOne(a => a.Workspace)
            .WithMany();
    }
}