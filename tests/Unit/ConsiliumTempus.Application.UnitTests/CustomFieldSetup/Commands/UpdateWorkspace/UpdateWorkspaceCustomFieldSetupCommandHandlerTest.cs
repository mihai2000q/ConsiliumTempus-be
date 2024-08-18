using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.UpdateWorkspace;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.UpdateWorkspace;

public class UpdateWorkspaceCustomFieldSetupCommandHandlerTest
{
    #region Setup

    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly UpdateWorkspaceCustomFieldSetupCommandHandler _uut;

    public UpdateWorkspaceCustomFieldSetupCommandHandlerTest()
    {
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _uut = new UpdateWorkspaceCustomFieldSetupCommandHandler(
            _customFieldSetupRepository,
            _workspaceRepository,
            _currentUserProvider);
    }

    #endregion

    [Fact]
    public async Task
        HandleUpdateWorkspaceCustomFieldSetupCommand_WhenSuccessful_ShouldUpdateWorkspaceAndReturnSuccessResult()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateUpdateWorkspaceCustomFieldSetupCommand();

        var customFieldSetup = CustomFieldSetupFactory.CreateText();
        _customFieldSetupRepository
            .GetWithWorkspace(Arg.Any<CustomFieldSetupId>())
            .Returns(customFieldSetup);

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));

        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.WorkspaceId));

        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateWorkspaceCustomFieldSetupResult());

        customFieldSetup.Workspace.Should().Be(workspace);
        customFieldSetup.Audit.ShouldBeUpdated(user);
        workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, Utils.TimeSpanPrecision);
    }

    [Fact]
    public async Task
        HandleUpdateWorkspaceCustomFieldSetupCommand_WhenWorkspaceIsNull_ShouldReturnWorkspaceNotFoundError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateUpdateWorkspaceCustomFieldSetupCommand();

        var customFieldSetup = CustomFieldSetupFactory.CreateText();
        _customFieldSetupRepository
            .GetWithWorkspace(Arg.Any<CustomFieldSetupId>())
            .Returns(customFieldSetup);

        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));

        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => id.Value == command.WorkspaceId));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.Workspace.NotFound);
    }

    [Fact]
    public async Task HandleUpdateWorkspaceCustomFieldSetupCommand_WhenIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateUpdateWorkspaceCustomFieldSetupCommand();

        _customFieldSetupRepository
            .GetWithWorkspace(Arg.Any<CustomFieldSetupId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));
        _workspaceRepository.DidNotReceive();
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.CustomFieldSetup.NotFound);
    }
}