using ConsiliumTempus.ArchitectureTests.Core;
using ConsiliumTempus.ArchitectureTests.TestUtils;
using ConsiliumTempus.Domain.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace ConsiliumTempus.ArchitectureTests.Layers;

public class ApplicationLayerTest : BaseArchitectureTest
{
    private const string BehaviorNamePattern = @"Behavior(`\d+)?$";

    [Fact]
    public void Application_ShouldNotHaveDependencyOnApi()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(ApplicationLayer)
            .Should()
            .NotDependOnAny(ApiLayer)
            .Check(Architecture);
    }

    [Fact]
    public void Application_ShouldNotHaveDependencyOnInfrastructure()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(ApplicationLayer)
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
            .Are(ApplicationLayer)
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .Check(Architecture);
    }

    [Fact]
    public void Requests_ShouldHaveSuffixCommandOrQuery()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(IRequest<>))
            .Should()
            .HaveNameEndingWith("Command")
            .OrShould()
            .HaveNameEndingWith("Query")
            .Check(Architecture);
    }

    [Fact]
    public void Validators_ShouldHaveSuffixValidator()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(AbstractValidator<>))
            .Should()
            .HaveNameEndingWith("Validator")
            .Check(Architecture);
    }

    [Fact]
    public void RequestHandlers_ShouldHaveSuffixCommandOrQueryHandler()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(IRequestHandler<,>))
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .OrShould()
            .HaveNameEndingWith("QueryHandler")
            .Check(Architecture);
    }

    [Fact]
    public void NotificationHandlers_ShouldHaveSuffixHandler()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(INotificationHandler<>))
            .Should()
            .HaveNameEndingWith("Handler")
            .Check(Architecture);
    }

    [Fact]
    public void NotificationHandlers_ShouldResideInEventsNamespace()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(INotificationHandler<>))
            .Should()
            .ResideInNamespaceMatching(@"^ConsiliumTempus\.Application\.\w+\.Events$")
            .Check(Architecture);
    }

    [Fact]
    public void DomainEvents_ShouldHaveExactlyOneHandlerNamedAfterThem()
    {
        var events = ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(IDomainEvent))
            .GetObjects(Architecture)
            .Select(e => $"{e.Name}Handler");

        var handlers = ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(INotificationHandler<>))
            .GetObjects(Architecture)
            .Select(h => h.Name);

        handlers.Should().BeEquivalentTo(events);
    }

    [Fact]
    public void Behaviors_ShouldHaveSuffixBehavior()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(IPipelineBehavior<,>))
            .Should()
            // Generic types have the arity at the end of their name (i.e., "ValidationBehavior`2")
            .HaveNameMatching(BehaviorNamePattern)
            .Check(Architecture);

        ArchRuleDefinition
            .Classes()
            .That()
            .ResideInNamespace("ConsiliumTempus.Application.Common.Behaviors")
            .Should()
            .HaveNameMatching(BehaviorNamePattern)
            .Check(Architecture);

        Utils.ShouldHaveSameTypes(
            Architecture,
            typeof(IPipelineBehavior<,>),
            "ConsiliumTempus.Application.Common.Behaviors");
    }

    [Fact]
    public void Extensions_ShouldHaveSuffixExtensions()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ResideInNamespace("ConsiliumTempus.Application.Common.Extensions")
            .And()
            .AreNotNested()
            .Should()
            .HaveNameEndingWith("Extensions")
            .Check(Architecture);
    }
}