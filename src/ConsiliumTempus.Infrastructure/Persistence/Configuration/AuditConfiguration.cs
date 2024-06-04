using ConsiliumTempus.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.ToTable(nameof(Audit));

        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.CreatedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Navigation(a => a.CreatedBy).AutoInclude();

        builder.HasOne(a => a.UpdatedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Navigation(a => a.UpdatedBy).AutoInclude();
    }
}