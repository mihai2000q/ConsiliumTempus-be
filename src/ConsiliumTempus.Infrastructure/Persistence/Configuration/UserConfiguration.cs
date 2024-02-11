using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public sealed class UserConfiguration : IEntityTypeConfiguration<UserAggregate>
{
    public void Configure(EntityTypeBuilder<UserAggregate> builder)
    {
        ConfigureUsersTable(builder);
    }

    private static void ConfigureUsersTable(EntityTypeBuilder<UserAggregate> builder)
    {
        builder.ToTable("User");

        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Id);
        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.OwnsOne(u => u.Credentials, cb =>
        {
            cb.HasIndex(c => c.Email)
                .IsUnique();
            cb.Property(c => c.Email)
                .HasColumnName("Email")
                .HasMaxLength(PropertiesValidation.User.EmailMaximumLength);

            cb.Property(c => c.Password)
                .HasColumnName("Password");
        });

        builder.OwnsOne(u => u.Name, nb =>
        {
            nb.Property(u => u.First)
                .HasColumnName("FirstName")
                .HasMaxLength(PropertiesValidation.User.FirstNameMaximumLength);

            nb.Property(u => u.Last)
                .HasColumnName("LastName")
                .HasMaxLength(PropertiesValidation.User.LastNameMaximumLength);
        });
    }
}