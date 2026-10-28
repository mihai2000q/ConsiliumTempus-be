using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Commands.UpdateStatus;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.UpdateStatus;

public class UpdateStatusFromProjectCommandHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly UpdateStatusFromProjectCommandHandler _uut;

    public UpdateStatusFromProjectCommandHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new UpdateStatusFromProjectCommandHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdateStatusFromProjectCommand_WhenIsSuccessful_ShouldUpdateStatusFromProject()
    {
        // Arrange
        var project = ProjectFactory.CreateWithStatuses();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommand(
            project.Id.Value,
            project.Statuses[0].Id.Value);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateStatusFromProjectResult());

        Utils.Project.AssertFromUpdateStatusCommand(project, command);
    }

    [Fact]
    public async Task HandleUpdateStatusFromProjectCommand_WhenProjectStatusIsNull_ShouldReturnStatusNotFoundError()
    {
        // Arrange
        var project = ProjectFactory.CreateWithStatuses();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommand(project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.ValidateError(Errors.ProjectStatus.NotFound);
    }

    [Fact]
    public async Task HandleUpdateStatusFromProjectCommand_WhenProjectIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommand();

        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.ValidateError(Errors.Project.NotFound);
    }
}