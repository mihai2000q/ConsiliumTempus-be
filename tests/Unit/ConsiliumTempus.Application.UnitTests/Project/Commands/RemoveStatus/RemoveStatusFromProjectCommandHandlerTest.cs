using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Commands.RemoveStatus;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.RemoveStatus;

public class RemoveStatusFromProjectCommandHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly RemoveStatusFromProjectCommandHandler _uut;

    public RemoveStatusFromProjectCommandHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new RemoveStatusFromProjectCommandHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleRemoveStatusFromProjectCommand_WhenIsSuccessful_ShouldRemoveStatusFromProject()
    {
        // Arrange
        var project = ProjectFactory.CreateWithStatuses();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateRemoveStatusFromProjectCommand(
            id: project.Id.Value,
            statusId: project.Statuses[0].Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new RemoveStatusFromProjectResult());

        Utils.Project.AssertFromRemoveStatusCommand(project, command);
    }

    [Fact]
    public async Task HandleRemoveStatusFromProjectCommand_WhenProjectStatusIsNull_ShouldReturnStatusNotFoundError()
    {
        // Arrange
        var project = ProjectFactory.CreateWithStatuses();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateRemoveStatusFromProjectCommand(id: project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.ValidateError(Errors.ProjectStatus.NotFound);
    }

    [Fact]
    public async Task HandleRemoveStatusFromProjectCommand_WhenProjectIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateRemoveStatusFromProjectCommand();

        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.ValidateError(Errors.Project.NotFound);
    }
}