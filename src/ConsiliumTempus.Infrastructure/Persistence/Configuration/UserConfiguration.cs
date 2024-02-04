using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.UserAggregate;
using ConsiliumTempus.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
    }

    private static void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));
        
        builder.HasIndex(u => u.Credentials.Email)
            .IsUnique();
        builder.Property(u => u.Credentials.Email)
            .HasMaxLength(PropertiesValidation.User.EmailMaximumLength);

        builder.Property(u => u.Credentials.Password);

        builder.Property(u => u.Name.First)
            .HasColumnName("FirstName")
            .HasMaxLength(PropertiesValidation.User.FirstNameMaximumLength);

        builder.Property(u => u.Name.Last)
            .HasColumnName("LastName")
            .HasMaxLength(PropertiesValidation.User.LastNameMaximumLength);
        
    }
}