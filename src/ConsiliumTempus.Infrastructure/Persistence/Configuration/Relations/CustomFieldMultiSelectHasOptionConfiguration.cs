using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Relations;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration.Relations;

public sealed class
    CustomFieldMultiSelectHasOptionConfiguration : IEntityTypeConfiguration<CustomFieldMultiSelectHasOption>
{
    public void Configure(EntityTypeBuilder<CustomFieldMultiSelectHasOption> builder)
    {
        builder.ToTable("CustomField_MultiSelectHasOption");

        /*builder.HasKey(c => new { c.MultiSelectCustomFieldId, c.OptionsId });
        builder.HasOne<CustomField>()
            .WithMany()
            .HasForeignKey(c => c.MultiSelectCustomFieldId);*/

        builder.HasOne<MultiSelectOption>()
            .WithMany()
            .HasForeignKey(c => c.OptionsId)
            .OnDelete(DeleteBehavior.NoAction);

        /*builder.Property(c => c.MultiSelectCustomFieldId)
            .HasConversion(
                id => id.Value,
                value => CustomFieldId.Create(value))
            .HasColumnName(nameof(CustomFieldId));*/

        builder.Property("MultiSelectCustomFieldId")
            .HasColumnName(nameof(CustomFieldId));

        builder.Property(c => c.OptionsId)
            .HasColumnName("OptionId");
    }
}