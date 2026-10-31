using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.MakeGlobal;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.MakeGlobal;

public class MakeCustomFieldSetupGlobalCommandHandlerTest
{
    #region Setup

    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly MakeCustomFieldSetupGlobalCommandHandler _uut;

    public MakeCustomFieldSetupGlobalCommandHandlerTest()
    {
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _uut = new MakeCustomFieldSetupGlobalCommandHandler(
            _customFieldSetupRepository);
    }

    #endregion

    [Fact]
    public async Task
        HandleMakeCustomFieldSetupGlobalCommand_WhenSuccessful_ShouldUpdateCustomFieldSetupAndReturnSuccessResult()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateMakeCustomFieldSetupGlobalCommand();

        var workspace = WorkspaceFactory.Create();
        var customFieldSetup = CustomFieldSetupFactory.CreateText(project: ProjectFactory.Create(workspace: workspace));
        _customFieldSetupRepository
            .GetWithWorkspaceAndProjects(Arg.Any<CustomFieldSetupId>())
            .Returns(customFieldSetup);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspaceAndProjects(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new MakeCustomFieldSetupGlobalResult());

        customFieldSetup.Workspace.Should().BeSameAs(workspace);
        workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, Utils.TimeSpanPrecision);
    }

    [Fact]
    public async Task
        HandleMakeCustomFieldSetupGlobalCommand_WhenItHasWorkspace_ShouldReturnAlreadyGlobalError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateMakeCustomFieldSetupGlobalCommand();

        var customFieldSetup = CustomFieldSetupFactory.CreateText(workspace: WorkspaceFactory.Create());
        _customFieldSetupRepository
            .GetWithWorkspaceAndProjects(Arg.Any<CustomFieldSetupId>())
            .Returns(customFieldSetup);

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspaceAndProjects(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.CustomFieldSetup.AlreadyGlobal);
    }

    [Fact]
    public async Task HandleMakeCustomFieldSetupGlobalCommand_WhenCustomFieldSetupIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateMakeCustomFieldSetupGlobalCommand();

        _customFieldSetupRepository
            .GetWithWorkspaceAndProjects(Arg.Any<CustomFieldSetupId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, CancellationToken.None);

        // Assert
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspaceAndProjects(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.CustomFieldSetup.NotFound);
    }
}