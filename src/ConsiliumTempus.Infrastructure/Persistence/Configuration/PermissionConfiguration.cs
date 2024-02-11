using ConsiliumTempus.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(nameof(Permission));
        
        builder.HasKey(p => p.Id);

        builder.HasData(Enum
            .GetValues<Domain.Common.Enums.Permissions>()
            .Select(p => Permission.Create((int)p + 1, p.ToString())));
    }
}