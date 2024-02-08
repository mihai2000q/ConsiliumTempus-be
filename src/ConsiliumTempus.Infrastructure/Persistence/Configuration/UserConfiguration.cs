using ConsiliumTempus.Domain.Common.Relations;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsiliumTempus.Infrastructure.Persistence.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<UserAggregate>
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

        builder.Ignore(u => u.Workspaces); // resolved in UserToWorkspaceConfiguration
    }
}