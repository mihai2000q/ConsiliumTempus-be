using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class SingleSelectOptionConfiguration : IEntityTypeConfiguration<SingleSelectOption>
{
    public void Configure(EntityTypeBuilder<SingleSelectOption> builder)
    {
        builder.ToTable(nameof(SingleSelectOption));

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Value)
            .HasMaxLength(PropertiesValidation.SingleSelectOption.ValueMaximumLength);

        builder.Property(o => o.Color);
        builder.Property(o => o.OrderPosition);
    }
}