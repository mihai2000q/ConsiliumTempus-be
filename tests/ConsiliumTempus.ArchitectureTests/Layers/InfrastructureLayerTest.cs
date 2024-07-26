using ConsiliumTempus.ArchitectureTests.Core;
using ConsiliumTempus.ArchitectureTests.TestUtils;

namespace ConsiliumTempus.ArchitectureTests.Layers;

public class InfrastructureLayerTest : BaseArchitectureTest
{
    [Fact]
    public void Infrastructure_Should_NotHaveDependencyOnApi()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(InfrastructureLayer)
            .Should()
            .NotDependOnAny(ApiLayer)
            .Check(Architecture);
    }

    [Fact]
    public void Infrastructure_Should_NotHaveDependencyOnApplication()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(InfrastructureLayer)
            .Should()
            .NotDependOnAny(ArchRuleDefinition
                .Types()
                .That()
                .Are(ApplicationLayer)
                .And()
                .AreNotInterfaces())
            .Check(Architecture);
    }

    [Fact]
    public void Classes_ShouldBeSealed()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .Are(InfrastructureLayer)
            .And()
            .DoNotResideInNamespace("ConsiliumTempus.Infrastructure.Migrations")
            .Should()
            .BeSealed()
            .Check(Architecture);
    }

    [Fact]
    public void Configurations_ShouldHaveSuffixConfiguration()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ResideInNamespace("ConsiliumTempus.Infrastructure.Persistence.Configuration")
            .Should()
            .HaveNameEndingWith("Configuration")
            .Check(Architecture);
    }

    [Fact]
    public void Interceptors_ShouldHaveSuffixInterceptor()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ResideInNamespace("ConsiliumTempus.Infrastructure.Persistence.Interceptors")
            .Should()
            .HaveNameEndingWith("Interceptor")
            .Check(Architecture);
    }

    [Fact]
    public void Repositories_ShouldHaveSuffixRepository()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ResideInNamespace("ConsiliumTempus.Infrastructure.Persistence.Repository")
            .Should()
            .HaveNameEndingWith("Repository")
            .Check(Architecture);
    }

    [Fact]
    public void Extensions_ShouldHaveSuffixExtensions()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ResideInNamespace("ConsiliumTempus.Infrastructure.Extensions")
            .Should()
            .HaveNameEndingWith("Extensions")
            .Check(Architecture);
    }
}