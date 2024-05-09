using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project.Entities.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Sprint.Commands.Update;

public class UpdateProjectSprintCommandHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly UpdateProjectSprintCommandHandler _uut;

    public UpdateProjectSprintCommandHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new UpdateProjectSprintCommandHandler(_projectSprintRepository);
    }

    #endregion

    [Fact]
    public async Task WhenUpdateProjectSprintIsSuccessful_ShouldUpdateAndReturnSuccessResult()
    {
        // Arrange
        var sprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetWithProjectAndWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(sprint);
        
        var command = ProjectSprintCommandFactory.CreateUpdateProjectSprintCommand(id: sprint.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithProjectAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateProjectSprintResult());
        
        Utils.ProjectSprint.AssertFromUpdateCommand(sprint, command);
    }

    [Fact]
    public async Task WhenUpdateProjectSprintFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateUpdateProjectSprintCommand();

        _projectSprintRepository
            .GetWithProjectAndWorkspace(Arg.Any<ProjectSprintId>())
            .ReturnsNull();
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithProjectAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }
}