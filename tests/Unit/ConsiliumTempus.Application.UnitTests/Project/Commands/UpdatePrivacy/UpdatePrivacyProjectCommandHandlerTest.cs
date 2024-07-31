using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Commands.UpdatePrivacy;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.UpdatePrivacy;

public class UpdatePrivacyProjectCommandHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly UpdatePrivacyProjectCommandHandler _uut;

    public UpdatePrivacyProjectCommandHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new UpdatePrivacyProjectCommandHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdatePrivacyProjectCommand_WhenIsSuccessful_ShouldUpdatePrivacyProjectAndReturnSuccess()
    {
        // Arrange
        var project = ProjectFactory.Create();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateUpdatePrivacyProjectCommand(id: project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdatePrivacyProjectResult());

        Utils.Project.AssertFromUpdatePrivacyCommand(project, command);
    }

    [Fact]
    public async Task HandleUpdatePrivacyProjectCommand_WhenProjectIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateUpdatePrivacyProjectCommand();

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