using ConsiliumTempus.Application.Common.Extensions;
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
        builder.ToTable(nameof(UserAggregate).TruncateAggregate());

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.OwnsOne(u => u.Credentials, cb =>
        {
            cb.HasIndex(c => c.Email)
                .IsUnique();
            cb.Property(c => c.Email)
                .HasColumnName(nameof(Credentials.Email))
                .HasMaxLength(PropertiesValidation.User.EmailMaximumLength);

            cb.Property(c => c.Password)
                .HasColumnName(nameof(Credentials.Password));
        });

        builder.OwnsOne(u => u.FirstName)
            .Property(fn => fn.Value)
            .HasColumnName(nameof(FirstName))
            .HasMaxLength(PropertiesValidation.User.FirstNameMaximumLength);

        builder.OwnsOne(u => u.LastName)
            .Property(ln => ln.Value)
            .HasColumnName(nameof(LastName))
            .HasMaxLength(PropertiesValidation.User.LastNameMaximumLength);

        builder.OwnsOne(u => u.Role)
            .Property(r => r.Value)
            .HasColumnName(nameof(Role))
            .HasMaxLength(PropertiesValidation.User.RoleMaximumLength);
    }
}