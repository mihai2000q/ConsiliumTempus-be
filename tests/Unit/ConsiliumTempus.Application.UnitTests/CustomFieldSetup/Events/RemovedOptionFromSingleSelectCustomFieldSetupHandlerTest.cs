using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.CustomFieldSetup.Events;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.ProjectTask.Entities;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using ConsiliumTempus.Domain.ProjectTask.Entities;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Events;

public class RemovedOptionFromSingleSelectCustomFieldSetupHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly RemovedOptionFromSingleSelectCustomFieldSetupHandler _uut;

    public RemovedOptionFromSingleSelectCustomFieldSetupHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new RemovedOptionFromSingleSelectCustomFieldSetupHandler(_projectTaskRepository);
    }

    #endregion

    [Fact]
    public async Task
        HandleRemovedOptionFromSingleSelectCustomFieldSetup_WhenSuccessful_ShouldClearTheOptionOnlyFromTheCustomFieldsThatHaveIt()
    {
        // Arrange
        var customFieldSetup = CustomFieldSetupFactory.CreateSingleSelect();
        var option = customFieldSetup.Options[0];
        var otherOption = customFieldSetup.Options[1];

        var customFieldWithOption = CustomFieldFactory.CreateSingleSelect(option.Id, customFieldSetup);
        var customFieldWithOtherOption = CustomFieldFactory.CreateSingleSelect(otherOption.Id, customFieldSetup);
        var customFieldWithoutOption = CustomFieldFactory.CreateSingleSelect(setup: customFieldSetup);
        var customFields = new List<SingleSelectCustomField>
        {
            customFieldWithOption,
            customFieldWithOtherOption,
            customFieldWithoutOption
        };
        _projectTaskRepository
            .GetSingleSelectCustomFieldsBySetup(customFieldSetup)
            .Returns(customFields);

        var domainEvent = new RemovedOptionFromSingleSelectCustomFieldSetup(customFieldSetup, option);

        // Act
        await _uut.Handle(domainEvent, CancellationToken.None);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetSingleSelectCustomFieldsBySetup(customFieldSetup);

        customFieldWithOption.Option.Should().BeNull();
        customFieldWithOtherOption.Option.Should().Be(otherOption);
        customFieldWithoutOption.Option.Should().BeNull();
    }
}
