using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.RemoveFromProject;

public class RemoveCustomFieldSetupFromProjectCommandHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly RemoveCustomFieldSetupFromProjectCommandHandler _uut;

    public RemoveCustomFieldSetupFromProjectCommandHandlerTest()
    {
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new RemoveCustomFieldSetupFromProjectCommandHandler(_projectRepository, _customFieldSetupRepository);
    }

    #endregion

    [Fact]
    public async Task
        HandleRemoveCustomFieldSetupFromProjectCommand_WhenSuccessful_ShouldRemoveProjectAndReturnSuccessResult()
    {
        // Arrange
        var project = ProjectFactory.Create();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        var customFieldSetup = CustomFieldSetupFactory.CreateText(
            workspace: WorkspaceFactory.Create(),
            project: project);
        _customFieldSetupRepository
            .GetWithWorkspaceAndProjects(Arg.Any<CustomFieldSetupId>())
            .Returns(customFieldSetup);
        
        var command = CustomFieldSetupCommandFactory.CreateRemoveCustomFieldSetupFromProjectCommand(
            customFieldSetup.Id.Value,
            project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspaceAndProjects(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));

        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => id.Value == command.ProjectId));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new RemoveCustomFieldSetupFromProjectResult());

        Utils.CustomFieldSetup.AssertRemoveFromProjectCommand(command, customFieldSetup, project);
    }

    [Fact]
    public async Task
        HandleRemoveCustomFieldSetupFromProjectCommand_WhenProjectIsNull_ShouldReturnProjectNotFoundError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateRemoveCustomFieldSetupFromProjectCommand();

        var customFieldSetup = CustomFieldSetupFactory.CreateText(workspace: WorkspaceFactory.Create());
        _customFieldSetupRepository
            .GetWithWorkspaceAndProjects(Arg.Any<CustomFieldSetupId>())
            .Returns(customFieldSetup);

        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspaceAndProjects(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));

        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => id.Value == command.ProjectId));

        outcome.ValidateError(Errors.Project.NotFound);
    }

    [Fact]
    public async Task HandleRemoveCustomFieldSetupFromProjectCommand_WhenItHasNoWorkspace_ShouldReturnNotGlobalError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateRemoveCustomFieldSetupFromProjectCommand();

        var customFieldSetup = CustomFieldSetupFactory.CreateText();
        _customFieldSetupRepository
            .GetWithWorkspaceAndProjects(Arg.Any<CustomFieldSetupId>())
            .Returns(customFieldSetup);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspaceAndProjects(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));
        _projectRepository.DidNotReceive();

        outcome.ValidateError(Errors.CustomFieldSetup.NotGlobal);
    }

    [Fact]
    public async Task HandleRemoveCustomFieldSetupFromProjectCommand_WhenIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateRemoveCustomFieldSetupFromProjectCommand();

        _customFieldSetupRepository
            .GetWithWorkspaceAndProjects(Arg.Any<CustomFieldSetupId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspaceAndProjects(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));
        _projectRepository.DidNotReceive();

        outcome.ValidateError(Errors.CustomFieldSetup.NotFound);
    }
}