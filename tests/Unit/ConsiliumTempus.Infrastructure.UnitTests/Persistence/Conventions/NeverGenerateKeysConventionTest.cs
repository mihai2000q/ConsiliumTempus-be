using ConsiliumTempus.Infrastructure.Persistence.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ConsiliumTempus.Infrastructure.UnitTests.Persistence.Conventions;

public class NeverGenerateKeysConventionTest
{
    #region Setup

    private sealed class GuidKeyEntity
    {
        public Guid Id { get; init; }
    }

    private sealed class IntKeyEntity
    {
        public int Id { get; init; }
    }

    private sealed class CompositeKeyEntity
    {
        public int First { get; init; }
        public int Second { get; init; }
    }

    private sealed class TestDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=NotUsed");
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new NeverGenerateKeysConvention());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuidKeyEntity>();
            modelBuilder.Entity<IntKeyEntity>();
            modelBuilder.Entity<CompositeKeyEntity>().HasKey(e => new { e.First, e.Second });
        }
    }

    private static IEntityType GetEntityType<TEntity>()
    {
        using var dbContext = new TestDbContext();

        return dbContext.Model.FindEntityType(typeof(TEntity))!;
    }

    #endregion

    [Fact]
    public void NeverGenerateKeys_WhenTheKeyIsAGuid_ShouldNeverGenerateIt()
    {
        // Act
        var key = GetEntityType<GuidKeyEntity>().FindProperty(nameof(GuidKeyEntity.Id))!;

        // Assert
        key.ValueGenerated.Should().Be(ValueGenerated.Never);
    }

    [Fact]
    public void NeverGenerateKeys_WhenTheKeyIsAnInteger_ShouldNeverGenerateItNorMakeItIdentity()
    {
        // Act
        var key = GetEntityType<IntKeyEntity>().FindProperty(nameof(IntKeyEntity.Id))!;

        // Assert
        key.ValueGenerated.Should().Be(ValueGenerated.Never);
        key.GetValueGenerationStrategy().Should().Be(SqlServerValueGenerationStrategy.None);
    }

    [Fact]
    public void NeverGenerateKeys_WhenTheKeyIsComposite_ShouldNeverGenerateAnyOfItsProperties()
    {
        // Act
        var keyProperties = GetEntityType<CompositeKeyEntity>().FindPrimaryKey()!.Properties;

        // Assert
        keyProperties.Should().HaveCount(2).And.OnlyContain(p => p.ValueGenerated == ValueGenerated.Never);
    }
}
