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
        builder.HasIndex(u => u.Id);
        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.OwnsOne(u => u.Credentials)
            .HasIndex(c => c.Email);
        builder.OwnsOne(u => u.Credentials)
            .Property(c => c.Email)
            .HasColumnName("Email")
            .HasMaxLength(PropertiesValidation.User.EmailMaximumLength);

        builder.OwnsOne(u => u.Credentials)
            .Property(c => c.Password)
            .HasColumnName("Password");
        
        builder.OwnsOne(u => u.Name)
            .Property(u => u.First)
            .HasColumnName("FirstName")
            .HasMaxLength(PropertiesValidation.User.FirstNameMaximumLength);
        
        builder.OwnsOne(u => u.Name)
            .Property(u => u.Last)
            .HasColumnName("LastName")
            .HasMaxLength(PropertiesValidation.User.LastNameMaximumLength);
    }
}