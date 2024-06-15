using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.Update;

public class UpdateProjectCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProjectRepository _projectRepository;
    private readonly UpdateProjectCommandHandler _uut;

    public UpdateProjectCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new UpdateProjectCommandHandler(_currentUserProvider, _projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdateProjectCommand_WhenIsSuccessful_ShouldUpdateProjectAndReturnSuccess()
    {
        // Arrange
        var project = ProjectFactory.CreateWithSprints();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        var currentUser = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(currentUser);

        var command = ProjectCommandFactory.CreateUpdateProjectCommand(id: project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateProjectResult());

        Utils.Project.AssertFromUpdateCommand(project, command, currentUser);
    }

    [Fact]
    public async Task HandleUpdateProjectCommand_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateUpdateProjectCommand();

        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => id.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.Project.NotFound);
    }
}