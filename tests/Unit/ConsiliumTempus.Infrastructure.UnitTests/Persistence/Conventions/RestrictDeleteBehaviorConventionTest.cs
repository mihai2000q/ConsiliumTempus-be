using ConsiliumTempus.Infrastructure.Persistence.Conventions;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.UnitTests.Persistence.Conventions;

public class RestrictDeleteBehaviorConventionTest
{
    #region Setup

    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class Principal
    {
        public Guid Id { get; init; }
    }

    private sealed class RequiredChild
    {
        public Guid Id { get; init; }
        public Guid PrincipalId { get; init; }
        public Principal Principal { get; init; } = null!;
    }

    private sealed class OptionalChild
    {
        public Guid Id { get; init; }
        public Guid? PrincipalId { get; init; }
        public Principal? Principal { get; init; }
    }

    private sealed class CascadeChild
    {
        public Guid Id { get; init; }
        public Guid PrincipalId { get; init; }
        public Principal Principal { get; init; } = null!;
    }

    private sealed class SetNullChild
    {
        public Guid Id { get; init; }
        public Guid? PrincipalId { get; init; }
        public Principal? Principal { get; init; }
    }

    private sealed class Owner
    {
        public Guid Id { get; init; }
        public OwnedValue Owned { get; init; } = null!;
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class OwnedValue
    {
        public required string Value { get; init; }
    }

    private class Base
    {
        public Guid Id { get; init; }
    }

    private sealed class Derived : Base;

    private sealed class TestDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=NotUsed");
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new RestrictDeleteBehaviorConvention());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Principal>()
                .Property(p => p.Id);
            modelBuilder.Entity<RequiredChild>()
                .HasOne(c => c.Principal)
                .WithMany()
                .IsRequired();
            modelBuilder.Entity<OptionalChild>()
                .HasOne(c => c.Principal)
                .WithMany()
                .IsRequired(false);
            modelBuilder.Entity<CascadeChild>()
                .HasOne(c => c.Principal)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SetNullChild>()
                .HasOne(c => c.Principal)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Owner>()
                .OwnsOne(o => o.Owned)
                .Property(o => o.Value)
                .HasMaxLength(200);
            modelBuilder.Entity<Base>().UseTptMappingStrategy();
            modelBuilder.Entity<Derived>();
        }
    }

    private static DeleteBehavior GetDeleteBehavior<TDependent>()
    {
        using var dbContext = new TestDbContext();

        return dbContext.Model
            .FindEntityType(typeof(TDependent))!
            .GetDeclaredForeignKeys()
            .Single()
            .DeleteBehavior;
    }

    #endregion

    [Fact]
    public void RestrictDeleteBehavior_WhenTheRelationshipIsRequired_ShouldRestrictTheDelete()
    {
        // Act
        var deleteBehavior = GetDeleteBehavior<RequiredChild>();

        // Assert
        deleteBehavior.Should().Be(DeleteBehavior.Restrict);
    }

    [Fact]
    public void RestrictDeleteBehavior_WhenTheRelationshipIsOptional_ShouldRestrictTheDelete()
    {
        // Act
        var deleteBehavior = GetDeleteBehavior<OptionalChild>();

        // Assert
        deleteBehavior.Should().Be(DeleteBehavior.Restrict);
    }

    [Fact]
    public void RestrictDeleteBehavior_WhenTheDeleteBehaviorIsConfiguredToCascade_ShouldKeepIt()
    {
        // Act
        var deleteBehavior = GetDeleteBehavior<CascadeChild>();

        // Assert
        deleteBehavior.Should().Be(DeleteBehavior.Cascade);
    }

    [Fact]
    public void RestrictDeleteBehavior_WhenTheDeleteBehaviorIsConfiguredToSetNull_ShouldKeepIt()
    {
        // Act
        var deleteBehavior = GetDeleteBehavior<SetNullChild>();

        // Assert
        deleteBehavior.Should().Be(DeleteBehavior.SetNull);
    }

    [Fact]
    public void RestrictDeleteBehavior_WhenTheRelationshipIsAnOwnership_ShouldNotChangeIt()
    {
        // Act
        var deleteBehavior = GetDeleteBehavior<OwnedValue>();

        // Assert
        deleteBehavior.Should().Be(DeleteBehavior.Cascade);
    }

    [Fact]
    public void RestrictDeleteBehavior_WhenTheTypeIsMappedToItsOwnTableAndInheritsFromAnother_ShouldCascadeTheDeleteOfTheBase()
    {
        // Act
        var deleteBehavior = GetDeleteBehavior<Derived>();

        // Assert
        deleteBehavior.Should().Be(DeleteBehavior.Cascade);
    }
}
