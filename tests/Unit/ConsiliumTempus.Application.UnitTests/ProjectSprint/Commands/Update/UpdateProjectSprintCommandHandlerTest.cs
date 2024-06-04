using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Commands.Update;

public class UpdateProjectSprintCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly UpdateProjectSprintCommandHandler _uut;

    public UpdateProjectSprintCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new UpdateProjectSprintCommandHandler(_currentUserProvider, _projectSprintRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdateProjectSprintCommand_WhenIsSuccessful_ShouldUpdateAndReturnSuccessResult()
    {
        // Arrange
        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);
        
        var sprint = ProjectSprintFactory.Create(createdBy: user);
        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);
        
        var command = ProjectSprintCommandFactory.CreateUpdateProjectSprintCommand(id: sprint.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateProjectSprintResult());
        
        Utils.ProjectSprint.AssertFromUpdateCommand(sprint, command, user);
    }

    [Fact]
    public async Task HandleUpdateProjectSprintCommand_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateUpdateProjectSprintCommand();

        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .ReturnsNull();
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }
}