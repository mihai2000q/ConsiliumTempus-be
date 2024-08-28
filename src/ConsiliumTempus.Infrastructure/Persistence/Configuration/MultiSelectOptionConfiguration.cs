using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class MultiSelectOptionConfiguration : IEntityTypeConfiguration<MultiSelectOption>
{
    public void Configure(EntityTypeBuilder<MultiSelectOption> builder)
    {
        builder.ToTable(nameof(MultiSelectOption));

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Value)
            .HasMaxLength(PropertiesValidation.MultiSelectOption.ValueMaximumLength);

        builder.Property(o => o.Color);

        builder.OwnsOne(o => o.CustomOrderPosition)
            .Property(c => c.Value)
            .HasColumnName(nameof(CustomOrderPosition));
    }
}