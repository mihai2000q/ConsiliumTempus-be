using ConsiliumTempus.ArchitectureTests.Core;
using ConsiliumTempus.ArchitectureTests.TestUtils;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.ArchitectureTests.Layers;

public class DomainLayerTest : BaseArchitectureTest
{
    private const string DomainEventsNamespacePattern = @"^ConsiliumTempus\.Domain\.\w+\.Events$";

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
            .AreNotAssignableTo(
                typeof(IDomainEvent),
                typeof(Filter<>),
                typeof(Order<>),
                typeof(FilterProperty<>),
                typeof(OrderProperty<>))
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
            .AreNot(typeof(Entity<>), typeof(AggregateRoot<>))
            .And()
            .AreNotAbstract()
            .GetObjects(Architecture)
            .ShouldHavePrivateParameterlessConstructor();
    }

    [Fact]
    public void Aggregates_ShouldHavePrivateParameterlessConstructor()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(AggregateRoot<>))
            .And()
            .AreNot(typeof(AggregateRoot<>))
            .And()
            .AreNotAbstract()
            .GetObjects(Architecture)
            .ShouldHavePrivateParameterlessConstructor();
    }

    [Fact]
    public void DomainEvents_ShouldResideInEventsNamespace()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(IDomainEvent))
            .Should()
            .ResideInNamespaceMatching(DomainEventsNamespacePattern)
            .Check(Architecture);

        ArchRuleDefinition
            .Classes()
            .That()
            .ResideInNamespaceMatching(DomainEventsNamespacePattern)
            .Should()
            .BeAssignableTo(typeof(IDomainEvent))
            .Check(Architecture);
    }

    [Fact]
    public void Aggregates_ShouldHaveSuffixAggregate()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(AggregateRoot<>))
            .And()
            .AreNot(typeof(AggregateRoot<>))
            .Should()
            .HaveNameEndingWith("Aggregate")
            .Check(Architecture);
    }
}