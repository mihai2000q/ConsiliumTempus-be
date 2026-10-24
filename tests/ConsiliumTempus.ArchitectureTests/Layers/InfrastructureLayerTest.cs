using ConsiliumTempus.ArchitectureTests.Core;
using ConsiliumTempus.ArchitectureTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
            .AreAssignableTo(typeof(IEntityTypeConfiguration<>))
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

        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(IInterceptor))
            .Should()
            .HaveNameEndingWith("Interceptor")
            .Check(Architecture);

        Utils.ShouldHaveSameTypes(
            Architecture,
            typeof(IInterceptor),
            "ConsiliumTempus.Infrastructure.Persistence.Interceptors");
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

        const string repositoryInterfacesNamespace =
            "ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository";

        var implementations = ArchRuleDefinition
            .Classes()
            .That()
            .Are(InfrastructureLayer)
            .GetObjects(Architecture)
            .Where(c => c.ImplementedInterfaces.Any(i => i.FullName.StartsWith($"{repositoryInterfacesNamespace}.")))
            .Select(c => c.FullName);

        var repositories = ArchRuleDefinition
            .Classes()
            .That()
            .ResideInNamespace("ConsiliumTempus.Infrastructure.Persistence.Repository")
            .GetObjects(Architecture)
            .Select(c => c.FullName);

        implementations.Should().BeEquivalentTo(repositories);
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