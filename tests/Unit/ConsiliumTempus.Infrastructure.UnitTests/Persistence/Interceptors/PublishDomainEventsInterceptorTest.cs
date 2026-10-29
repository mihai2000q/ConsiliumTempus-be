using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Infrastructure.Persistence.Interceptors;
using ConsiliumTempus.Infrastructure.UnitTests.TestUtils;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ConsiliumTempus.Infrastructure.UnitTests.Persistence.Interceptors;

public class PublishDomainEventsInterceptorTest
{
    private readonly IPublisher _publisher = Substitute.For<IPublisher>();
    private readonly PublishDomainEventsInterceptor _uut;

    public PublishDomainEventsInterceptorTest()
    {
        _uut = new PublishDomainEventsInterceptor(_publisher);
    }

    [Fact]
    public async Task PublishDomainEvents_WhenEntitiesHaveDomainEvents_ShouldPublishAndClearThem()
    {
        // Arrange
        await using var dbContext = Utils.Persistence.CreateDbContext();
        var stage = ProjectStageFactory.Create();
        dbContext.Attach(stage);
        var stageEvent = Substitute.For<IDomainEvent>();
        var sprintEvent = Substitute.For<IDomainEvent>();
        stage.AddDomainEvent(stageEvent);
        stage.Sprint.AddDomainEvent(sprintEvent);
        using var cancellationTokenSource = new CancellationTokenSource();

        // Act
        await _uut.SavingChangesAsync(
            Utils.Persistence.CreateEventData(dbContext),
            default,
            cancellationTokenSource.Token);

        // Assert
        await _publisher.Received(1).Publish(stageEvent, cancellationTokenSource.Token);
        await _publisher.Received(1).Publish(sprintEvent, cancellationTokenSource.Token);
        stage.DomainEvents.Should().BeEmpty();
        stage.Sprint.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public async Task PublishDomainEvents_WhenTheDomainEventsRaiseMoreDomainEvents_ShouldPublishThemAll()
    {
        // Arrange
        await using var dbContext = Utils.Persistence.CreateDbContext();
        var stage = ProjectStageFactory.Create();
        dbContext.Attach(stage);
        var firstEvent = Substitute.For<IDomainEvent>();
        var secondEvent = Substitute.For<IDomainEvent>();
        stage.AddDomainEvent(firstEvent);
        _publisher
            .When(p => p.Publish(firstEvent, Arg.Any<CancellationToken>()))
            .Do(_ => stage.AddDomainEvent(secondEvent));

        // Act
        await _uut.SavingChangesAsync(Utils.Persistence.CreateEventData(dbContext), default);

        // Assert
        Received.InOrder(() =>
        {
            _publisher.Publish(firstEvent, Arg.Any<CancellationToken>());
            _publisher.Publish(secondEvent, Arg.Any<CancellationToken>());
        });
        stage.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public async Task PublishDomainEvents_WhenThereAreNoDomainEvents_ShouldNotPublishAnything()
    {
        // Arrange
        await using var dbContext = Utils.Persistence.CreateDbContext();
        dbContext.Attach(ProjectStageFactory.Create());

        // Act
        await _uut.SavingChangesAsync(Utils.Persistence.CreateEventData(dbContext), default);

        // Assert
        await _publisher.DidNotReceiveWithAnyArgs().Publish(default(IDomainEvent)!);
    }

    [Fact]
    public async Task PublishDomainEvents_WhenThereIsNoDbContext_ShouldNotPublishAnything()
    {
        // Act
        var outcome = await _uut.SavingChangesAsync(Utils.Persistence.CreateEventData(null), default);

        // Assert
        outcome.Should().Be(default(InterceptionResult<int>));
        await _publisher.DidNotReceiveWithAnyArgs().Publish(default(IDomainEvent)!);
    }

    [Fact]
    public void PublishDomainEvents_WhenSavingSynchronously_ShouldPublishAndClearTheDomainEvents()
    {
        // Arrange
        using var dbContext = Utils.Persistence.CreateDbContext();
        var stage = ProjectStageFactory.Create();
        dbContext.Attach(stage);
        var domainEvent = Substitute.For<IDomainEvent>();
        stage.AddDomainEvent(domainEvent);

        // Act
        _uut.SavingChanges(Utils.Persistence.CreateEventData(dbContext), default);

        // Assert
        _publisher.Received(1).Publish(domainEvent, Arg.Any<CancellationToken>());
        stage.DomainEvents.Should().BeEmpty();
    }
}
