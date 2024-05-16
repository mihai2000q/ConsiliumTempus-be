using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Sprint.Commands.Delete;

public class DeleteProjectSprintCommandHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly DeleteProjectSprintCommandHandler _uut;

    public DeleteProjectSprintCommandHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new DeleteProjectSprintCommandHandler(_projectSprintRepository);
    }

    #endregion

    [Fact]
    public async Task HandleDeleteProjectSprintCommand_WhenIsSuccessful_ShouldRemoveAndReturnSuccessfulResult()
    {
        // Arrange
        var projectSprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(projectSprint);

        var command = ProjectSprintCommandFactory.CreateDeleteProjectSprintCommand(projectSprint.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _projectSprintRepository
            .Received(1)
            .Remove(Arg.Is<ProjectSprint>(ps => Utils.ProjectSprint.AssertFromDeleteCommand(ps, command)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteProjectSprintResult());
    }

    [Fact]
    public async Task HandleDeleteProjectSprintCommand_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectSprintCommandFactory.CreateDeleteProjectSprintCommand();

        _projectSprintRepository
            .GetWithWorkspace(Arg.Any<ProjectSprintId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _projectSprintRepository
            .DidNotReceive()
            .Remove(Arg.Any<ProjectSprint>());

        outcome.IsError.Should().BeTrue();
        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }
}