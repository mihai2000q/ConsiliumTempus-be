using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.ArchitectureTests.Core;
using ConsiliumTempus.ArchitectureTests.TestUtils;
using Mapster;

namespace ConsiliumTempus.ArchitectureTests.Layers;

public class ApiLayerTest : BaseArchitectureTest
{
    [Fact]
    public void Api_ShouldNotHaveDependencyOnInfrastructure()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(ApiLayer)
            .Should()
            .NotDependOnAny(InfrastructureLayer)
            .Check(Architecture);
    }

    [Fact]
    public void Classes_ShouldBeSealed()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .Are(ApiLayer)
            .And()
            .AreNotAbstract()
            .And()
            .AreNot(typeof(Program))
            .Should()
            .BeSealed()
            .Check(Architecture);
    }

    [Fact]
    public void Mappers_ShouldHaveSuffixMappingConfig()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(IRegister))
            .Should()
            .HaveNameEndingWith("MappingConfig")
            .Check(Architecture);

        ArchRuleDefinition
            .Classes()
            .That()
            .ResideInNamespace("ConsiliumTempus.Api.Common.Mapping")
            .Should()
            .HaveNameEndingWith("MappingConfig")
            .Check(Architecture);

        Utils.ShouldHaveSameCount(
            Architecture,
            typeof(IRegister),
            "ConsiliumTempus.Api.Common.Mapping");
    }

    [Fact]
    public void Contracts_ShouldHaveSuffixRequestOrResponse()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ResideInNamespace("ConsiliumTempus.Api.Contracts")
            .Should()
            .HaveNameEndingWith("Request")
            .OrShould()
            .HaveNameEndingWith("Response")
            .Check(Architecture);
    }

    [Fact]
    public void Controllers_ShouldHaveSuffixMappingConfig()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(ApiController))
            .Should()
            .HaveNameEndingWith("Controller")
            .Check(Architecture);

        ArchRuleDefinition
            .Classes()
            .That()
            .ResideInNamespace("ConsiliumTempus.Api.Controllers")
            .Should()
            .HaveNameEndingWith("Controller")
            .Check(Architecture);

        Utils.ShouldHaveSameCount(
            Architecture,
            typeof(ApiController),
            "ConsiliumTempus.Api.Controllers");
    }
}