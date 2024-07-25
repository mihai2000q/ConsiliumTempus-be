using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.ArchitectureTests.Layers;

public class DomainLayerTest : BaseArchitectureTest
{
    [Fact]
    public void Domain_ShouldNotHaveDependencyOnApi()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(DomainLayer)
            .Should()
            .NotDependOnAny(ApiLayer)
            .Check(Architecture);
    }

    [Fact]
    public void Domain_ShouldNotHaveDependencyOnApplication()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(DomainLayer)
            .Should()
            .NotDependOnAny(ApplicationLayer)
            .Check(Architecture);
    }

    [Fact]
    public void Domain_ShouldNotHaveDependencyOnInfrastructure()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(DomainLayer)
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
            .Are(DomainLayer)
            .And()
            .AreNotAbstract()
            .And()
            .AreNot(typeof(Filter<>), typeof(Order<>))
            .Should()
            .BeSealed()
            .Check(Architecture);
    }

    [Fact]
    public void Classes_ShouldHavePrivateConstructor()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .Are(DomainLayer)
            .And()
            .AreNotAbstract()
            .And()
            .AreNot(typeof(Filter<>), typeof(Order<>))
            .And()
            .AreNotAssignableTo(typeof(IDomainEvent), typeof(FilterProperty<>), typeof(OrderProperty<>))
            .GetObjects(Architecture)
            .ShouldHavePrivateConstructors();
    }

    [Fact]
    public void ValueObjects_ShouldHavePrivateParameterlessConstructor()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(ValueObject))
            .And()
            .AreNot(typeof(ValueObject), typeof(AggregateRootId<>))
            .GetObjects(Architecture)
            .ShouldHavePrivateParameterlessConstructor();
    }

    [Fact]
    public void Entities_ShouldHavePrivateParameterlessConstructor()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(Entity<>))
            .And()
            .AreNot(typeof(Entity<>), typeof(AggregateRoot<,>))
            .GetObjects(Architecture)
            .ShouldHavePrivateParameterlessConstructor();
    }

    [Fact]
    public void Aggregates_ShouldHavePrivateParameterlessConstructor()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(AggregateRoot<,>))
            .And()
            .AreNot(typeof(AggregateRoot<,>))
            .GetObjects(Architecture)
            .ShouldHavePrivateParameterlessConstructor();
    }
}