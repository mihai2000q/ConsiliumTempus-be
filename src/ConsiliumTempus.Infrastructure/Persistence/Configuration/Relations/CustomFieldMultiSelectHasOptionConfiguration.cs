using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Relations;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration.Relations;

internal sealed class
    CustomFieldMultiSelectHasOptionConfiguration : IEntityTypeConfiguration<CustomFieldMultiSelectHasOption>
{
    public void Configure(EntityTypeBuilder<CustomFieldMultiSelectHasOption> builder)
    {
        builder.ToTable("CustomField_MultiSelectHasOption");

        builder.HasKey(c => new { c.MultiSelectCustomFieldId, c.OptionId });

        builder.HasOne<MultiSelectCustomField>()
            .WithMany()
            .HasForeignKey(c => c.MultiSelectCustomFieldId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<MultiSelectOption>()
            .WithMany()
            .HasForeignKey(c => c.OptionId);

        builder.Property(c => c.MultiSelectCustomFieldId)
            .HasConversion(
                id => id.Value,
                value => CustomFieldId.Create(value))
            .HasColumnName(nameof(CustomFieldId));
    }
}