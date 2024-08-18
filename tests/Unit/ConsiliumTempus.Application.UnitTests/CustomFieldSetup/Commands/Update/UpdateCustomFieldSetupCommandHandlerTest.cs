using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Update;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Commands.Update;

public class UpdateCustomFieldSetupCommandHandlerTest
{
    #region Setup

    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly UpdateCustomFieldSetupCommandHandler _uut;

    public UpdateCustomFieldSetupCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _uut = new UpdateCustomFieldSetupCommandHandler(_customFieldSetupRepository, _currentUserProvider);
    }

    #endregion

    [Theory]
    [ClassData(typeof(UpdateCustomFieldSetupCommandHandlerData.GetCommands))]
    public async Task HandleUpdateCustomFieldSetupCommand_WhenSuccessful_ShouldUpdateAndSaveCustomFieldSetup(
        UpdateCustomFieldSetupCommand command,
        CustomFieldSetupAggregate customFieldSetup)
    {
        // Arrange
        _customFieldSetupRepository
            .GetWithWorkspaceAndProjects(Arg.Any<CustomFieldSetupId>())
            .Returns(customFieldSetup);

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspaceAndProjects(Arg.Is<CustomFieldSetupId>(cId => cId.Value == command.Id));

        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateCustomFieldSetupResult());

        Utils.CustomFieldSetup.AssertFromUpdateCommand(command, customFieldSetup, user);
    }

    [Fact]
    public async Task HandleUpdateCustomFieldSetupCommand_WhenIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand();

        _customFieldSetupRepository
            .GetWithWorkspaceAndProjects(Arg.Any<CustomFieldSetupId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _customFieldSetupRepository
            .Received(1)
            .GetWithWorkspaceAndProjects(Arg.Is<CustomFieldSetupId>(cId => cId.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.CustomFieldSetup.NotFound);
    }
}