using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Commands.AddStatus;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.AddStatus;

public class AddStatusToProjectCommandHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly AddStatusToProjectCommandHandler _uut;

    public AddStatusToProjectCommandHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new AddStatusToProjectCommandHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleAddStatusToProjectCommand_WhenIsSuccessful_ShouldCreateAndSaveProjectStatusOnProject()
    {
        // Arrange
        var project = ProjectFactory.Create();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateAddStatusToProjectCommand(project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new AddStatusToProjectResult());

        Utils.Project.AssertFromAddStatusCommand(project, command);
    }

    [Fact]
    public async Task HandleAddStatusToProjectCommand_WhenProjectIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateAddStatusToProjectCommand();

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