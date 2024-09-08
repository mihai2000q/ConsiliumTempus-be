using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.Create;

public class CreateCustomFieldSetupCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly CreateCustomFieldSetupCommandHandler _uut;

    public CreateCustomFieldSetupCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _uut = new CreateCustomFieldSetupCommandHandler(
            _currentUserProvider,
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

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);

        // Capture Arguments
        CustomFieldSetupAggregate? capturedCustomFieldSetup = null;
        _customFieldSetupRepository
            .When(c => c.Add(Arg.Any<CustomFieldSetupAggregate>()))
            .Do(res => capturedCustomFieldSetup = res.Arg<CustomFieldSetupAggregate>());

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        if (command.WorkspaceId is not null)
            await _workspaceRepository
                .Received(1)
                .Get(Arg.Is<WorkspaceId>(id => id.Value == command.WorkspaceId));
        else
            _workspaceRepository.DidNotReceive();

        if (command.ProjectId is not null)
            await _projectRepository
                .Received(1)
                .Get(Arg.Is<ProjectId>(id => id.Value == command.ProjectId));
        else
            _projectRepository.DidNotReceive();

        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        await _customFieldSetupRepository
            .Received(1)
            .Add(Arg.Any<CustomFieldSetupAggregate>());

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new CreateCustomFieldSetupResult());

        Utils.CustomFieldSetup.AssertFromCreateCommand(
            command,
            capturedCustomFieldSetup!,
            user,
            workspace,
            project);
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
        var outcome = await _uut.Handle(command, default);

        // Assert
        _workspaceRepository.DidNotReceive();
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => id.Value == command.ProjectId));
        _currentUserProvider.DidNotReceive();
        _customFieldSetupRepository.DidNotReceive();

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
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.WorkspaceId));
        _projectRepository.DidNotReceive();
        _currentUserProvider.DidNotReceive();
        _customFieldSetupRepository.DidNotReceive();

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}