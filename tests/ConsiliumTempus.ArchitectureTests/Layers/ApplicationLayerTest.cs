using ConsiliumTempus.ArchitectureTests.Core;
using FluentValidation;
using MediatR;

namespace ConsiliumTempus.ArchitectureTests.Layers;

public class ApplicationLayerTest : BaseArchitectureTest
{
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
    public void Behaviors_ShouldHaveSuffixBehavior()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(IPipelineBehavior<,>))
            .Should()
            // Should have been NameEndingWith, however it only works if you add: "`2" at the end
            .HaveNameContaining("Behavior") 
            .Check(Architecture);
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