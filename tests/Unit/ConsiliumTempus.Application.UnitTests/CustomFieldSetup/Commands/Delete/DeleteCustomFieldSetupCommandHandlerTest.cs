using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.Delete;

public class DeleteCustomFieldSetupCommandHandlerTest
{
    #region Setup

    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly DeleteCustomFieldSetupCommandHandler _uut;

    public DeleteCustomFieldSetupCommandHandlerTest()
    {
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _uut = new DeleteCustomFieldSetupCommandHandler(_customFieldSetupRepository);
    }

    #endregion

    [Fact]
    public async Task HandleDeleteCustomFieldSetupCommand_WhenSuccessful_ShouldDeleteAndReturnSuccessResult()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateDeleteCustomFieldSetupCommand();

        var customFieldSetup = CustomFieldSetupFactory.CreateText();
        _customFieldSetupRepository
            .GetWithWorkspaceAndProject(Arg.Any<CustomFieldSetupId>())
            .Returns(customFieldSetup);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspaceAndProject(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteCustomFieldSetupResult());

        customFieldSetup.Workspace?.LastActivity.Should().BeCloseTo(DateTime.UtcNow, Utils.TimeSpanPrecision);
        customFieldSetup.Project?.LastActivity.Should().BeCloseTo(DateTime.UtcNow, Utils.TimeSpanPrecision);
        customFieldSetup.Project?.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, Utils.TimeSpanPrecision);
    }

    [Fact]
    public async Task HandleDeleteCustomFieldSetupCommand_WhenIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateDeleteCustomFieldSetupCommand();

        _customFieldSetupRepository
            .GetWithWorkspaceAndProject(Arg.Any<CustomFieldSetupId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspaceAndProject(Arg.Is<CustomFieldSetupId>(id => id.Value == command.Id));

        outcome.ValidateError(Errors.CustomFieldSetup.NotFound);
    }
}