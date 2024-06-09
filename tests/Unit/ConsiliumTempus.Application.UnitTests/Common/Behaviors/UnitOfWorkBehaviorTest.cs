using ConsiliumTempus.Application.Common.Behaviors;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using MediatR;

namespace ConsiliumTempus.Application.UnitTests.Common.Behaviors;

public class UnitOfWorkBehaviorTest
{
    #region Setup

    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    #endregion

    [Fact]
    public async Task WhenItIsCommandAndSuccessful_ShouldSaveChangesAndInvokeNextBehavior()
    {
        // Arrange
        var uut = new UnitOfWorkBehavior<CreateProjectCommand, ErrorOr<CreateProjectResult>>(_unitOfWork);
        var nextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<CreateProjectResult>>>();

        var result = new CreateProjectResult();
        nextBehavior
            .Invoke()
            .Returns(result);

        var command = ProjectCommandFactory.CreateCreateProjectCommand();

        // Act
        var outcome = await uut.Handle(command, nextBehavior, default);

        // Assert
        await nextBehavior
            .Received(1)
            .Invoke();
        await _unitOfWork
            .Received(1)
            .SaveChangesAsync();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(result);
    }

    [Fact]
    public async Task WhenItIsCommandAndHasErrors_ShouldInvokeNextBehavior()
    {
        // Arrange
        var uut = new UnitOfWorkBehavior<CreateProjectCommand, ErrorOr<CreateProjectResult>>(_unitOfWork);
        var nextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<CreateProjectResult>>>();

        var error = Errors.Workspace.NotFound;
        nextBehavior
            .Invoke()
            .Returns(error);

        var command = ProjectCommandFactory.CreateCreateProjectCommand();

        // Act
        var outcome = await uut.Handle(command, nextBehavior, default);

        // Assert
        await nextBehavior
            .Received(1)
            .Invoke();
        _unitOfWork.DidNotReceive();

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task WhenItIsNotCommand_ShouldInvokeNextBehavior()
    {
        // Arrange
        var uut = new UnitOfWorkBehavior<GetWorkspaceQuery, ErrorOr<GetWorkspaceResult>>(_unitOfWork);
        var nextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<GetWorkspaceResult>>>();

        var result = WorkspaceResultFactory.CreateGetWorkspaceResult();
        nextBehavior
            .Invoke()
            .Returns(result);

        var query = WorkspaceQueryFactory.CreateGetWorkspaceQuery();

        // Act
        var outcome = await uut.Handle(query, nextBehavior, default);

        // Assert
        await nextBehavior
            .Received(1)
            .Invoke();
        _unitOfWork.DidNotReceive();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(result);
    }
}