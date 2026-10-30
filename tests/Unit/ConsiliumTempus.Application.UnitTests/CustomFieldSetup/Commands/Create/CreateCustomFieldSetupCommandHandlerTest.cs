using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.Create;

public class CreateCustomFieldSetupCommandHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly CreateCustomFieldSetupCommandHandler _uut;

    public CreateCustomFieldSetupCommandHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _uut = new CreateCustomFieldSetupCommandHandler(
            _workspaceRepository,
            _projectRepository,
            _customFieldSetupRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(CreateCustomFieldSetupCommandHandlerData.GetCommands))]
    public async Task HandleCreateCustomFieldSetupCommand_WhenSuccessful_ShouldCreateAndSaveCustomFieldSetup(
        CreateCustomFieldSetupCommand command)
    {
        // Arrange
        // Mock
        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var project = ProjectFactory.Create();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        // Capture Arguments
        CustomFieldSetupAggregate? capturedCustomFieldSetup = null;
        _customFieldSetupRepository
            .When(c => c.Add(Arg.Any<CustomFieldSetupAggregate>()))
            .Do(res => capturedCustomFieldSetup = res.Arg<CustomFieldSetupAggregate>());

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        if (command.WorkspaceId is not null)
            await _workspaceRepository
                .Received(1)
                .Get(Arg.Is<WorkspaceId>(id => id.Value == command.WorkspaceId));
        else
            await _workspaceRepository.DidNotReceive().Get(Arg.Any<WorkspaceId>());

        if (command.ProjectId is not null)
            await _projectRepository
                .Received(1)
                .Get(Arg.Is<ProjectId>(id => id.Value == command.ProjectId));
        else
            await _projectRepository.DidNotReceive().Get(Arg.Any<ProjectId>());

        await _customFieldSetupRepository
            .Received(1)
            .Add(Arg.Any<CustomFieldSetupAggregate>());

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new CreateCustomFieldSetupResult());

        Utils.CustomFieldSetup.AssertFromCreateCommand(
            command,
            capturedCustomFieldSetup!,
            workspace,
            project);
    }

    [Fact]
    public async Task
        HandleCreateCustomFieldSetupCommand_WhenCustomFieldTypeIsNotSupported_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
            type: (CustomFieldType)int.MaxValue);

        // Act
        var outcome = async () => await _uut.Handle(command, CancellationToken.None);

        // Assert
        await outcome.Should().ThrowAsync<ArgumentOutOfRangeException>();
        await _customFieldSetupRepository
            .DidNotReceive()
            .Add(Arg.Any<CustomFieldSetupAggregate>());
    }

    [Fact]
    public async Task HandleCreateCustomFieldSetupCommand_WhenProjectIsNull_ShouldReturnProjectNotFoundError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
            null,
            Guid.NewGuid());

        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _workspaceRepository.DidNotReceive().Get(Arg.Any<WorkspaceId>());
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => id.Value == command.ProjectId));
        await _customFieldSetupRepository.DidNotReceive().Add(Arg.Any<CustomFieldSetupAggregate>());

        outcome.ValidateError(Errors.Project.NotFound);
    }

    [Fact]
    public async Task HandleCreateCustomFieldSetupCommand_WhenWorkspaceIsNull_ShouldReturnWorkspaceNotFoundError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
            Guid.NewGuid());

        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.WorkspaceId));
        await _projectRepository.DidNotReceive().Get(Arg.Any<ProjectId>());
        await _customFieldSetupRepository.DidNotReceive().Add(Arg.Any<CustomFieldSetupAggregate>());

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}