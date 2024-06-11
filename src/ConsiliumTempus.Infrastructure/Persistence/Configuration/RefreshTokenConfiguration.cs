using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.Authentication.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable(nameof(RefreshToken));

        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id)
            .HasConversion(
                id => id.Value,
                value => RefreshTokenId.Create(value));

        builder.OwnsOne(rth => rth.IsInvalidated)
            .Property(i => i.Value)
            .HasColumnName(nameof(IsInvalidated));

        builder.HasOne(rt => rt.User)
            .WithMany();

        builder.OwnsMany(rt => rt.History, ConfigureHistory);
    }

    private static void ConfigureHistory(OwnedNavigationBuilder<RefreshToken, RefreshTokenHistory> builder)
    {
        builder.ToTable(nameof(RefreshTokenHistory));

        builder.HasKey(rth => rth.Id);

        builder.OwnsOne(rth => rth.JwtId)
            .Property(j => j.Value)
            .HasColumnName(nameof(JwtId));

        builder.HasOne(rth => rth.RefreshToken)
            .WithMany(rt => rt.History);
    }
}