using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.UpdateOverview;

public class UpdateOverviewProjectCommandHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly UpdateOverviewProjectCommandHandler _uut;

    public UpdateOverviewProjectCommandHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new UpdateOverviewProjectCommandHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdateOverviewProjectCommand_WhenIsSuccessful_ShouldUpdateOverviewProjectAndReturnSuccess()
    {
        // Arrange
        var project = ProjectFactory.CreateWithSprints();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateUpdateOverviewProjectCommand(id: project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateOverviewProjectResult());

        Utils.Project.AssertFromUpdateOverviewCommand(project, command);
    }

    [Fact]
    public async Task HandleUpdateOverviewProjectCommand_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateUpdateOverviewProjectCommand();

        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.Project.NotFound);
    }
}