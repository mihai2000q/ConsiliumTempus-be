using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.Update;

public class UpdateProjectCommandHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly UpdateProjectCommandHandler _uut;

    public UpdateProjectCommandHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new UpdateProjectCommandHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdateProjectCommand_WhenIsSuccessful_ShouldUpdateProjectAndReturnSuccess()
    {
        // Arrange
        var project = ProjectFactory.Create();
        _projectRepository
            .GetWithWorkspace(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateUpdateProjectCommand(id: project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateProjectResult());

        Utils.Project.AssertFromUpdateCommand(project, command);
    }

    [Fact]
    public async Task HandleUpdateProjectCommand_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateUpdateProjectCommand();

        _projectRepository
            .GetWithWorkspace(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.Project.NotFound);
    }
}