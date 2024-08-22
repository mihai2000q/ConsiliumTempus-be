using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.AddToProject;

public class AddCustomFieldSetupToProjectCommandHandlerTest
{
    #region Setup

    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly AddCustomFieldSetupToProjectCommandHandler _uut;

    public AddCustomFieldSetupToProjectCommandHandlerTest()
    {
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new AddCustomFieldSetupToProjectCommandHandler(_customFieldSetupRepository, _projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleAddCustomFieldSetupToProjectCommand_WhenSuccessful_ShouldAddProjectAndReturnSuccessResult()
    {
        // Arrange
        var customFieldSetup = CustomFieldSetupFactory.CreateText(workspace: WorkspaceFactory.Create());
        _customFieldSetupRepository
            .GetWithWorkspaceAndProjects(Arg.Any<CustomFieldSetupId>())
            .Returns(customFieldSetup);

        var project = ProjectFactory.Create();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        var command = CustomFieldSetupCommandFactory.CreateAddCustomFieldSetupToProjectCommand(
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
        outcome.Value.Should().Be(new AddCustomFieldSetupToProjectResult());

        Utils.CustomFieldSetup.AssertAddToProjectCommand(command, customFieldSetup, project);
    }

    [Fact]
    public async Task HandleAddCustomFieldSetupToProjectCommand_WhenItAlreadyHasProject_ShouldReturnProjectAlreadyPresentError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateAddCustomFieldSetupToProjectCommand();

        var customFieldSetup = CustomFieldSetupFactory.CreateText(workspace: WorkspaceFactory.Create());
        _customFieldSetupRepository
            .GetWithWorkspaceAndProjects(Arg.Any<CustomFieldSetupId>())
            .Returns(customFieldSetup);

        var project = ProjectFactory.Create();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        customFieldSetup.AddProject(project);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspaceAndProjects(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));

        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => id.Value == command.ProjectId));

        outcome.ValidateError(Errors.CustomFieldSetup.ProjectAlreadyPresent);
    }

    [Fact]
    public async Task HandleAddCustomFieldSetupToProjectCommand_WhenProjectIsNull_ShouldReturnProjectNotFoundError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateAddCustomFieldSetupToProjectCommand();

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
    public async Task HandleAddCustomFieldSetupToProjectCommand_WhenItHasNoWorkspace_ShouldReturnNotGlobalError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateAddCustomFieldSetupToProjectCommand();

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
    public async Task HandleAddCustomFieldSetupToProjectCommand_WhenIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateAddCustomFieldSetupToProjectCommand();

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