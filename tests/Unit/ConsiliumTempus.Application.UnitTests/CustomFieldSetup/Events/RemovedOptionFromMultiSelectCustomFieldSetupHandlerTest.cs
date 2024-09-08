using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.CustomFieldSetup.Events;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.ProjectTask.Entities;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using ConsiliumTempus.Domain.ProjectTask.Entities;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Events;

public class RemovedOptionFromMultiSelectCustomFieldSetupHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly RemovedOptionFromMultiSelectCustomFieldSetupHandler _uut;

    public RemovedOptionFromMultiSelectCustomFieldSetupHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new RemovedOptionFromMultiSelectCustomFieldSetupHandler(_projectTaskRepository);
    }

    #endregion

    [Fact]
    public async Task
        HandleRemovedOptionFromMultiSelectCustomFieldSetup_WhenSuccessful_ShouldRemoveOptionFromMultiSelectCustomField()
    {
        // Arrange
        var option = MultiSelectOptionFactory.Create();
        var customFieldSetup = CustomFieldSetupFactory.CreateMultiSelect(
            options:
            [
                MultiSelectOptionFactory.Create(),
                option,
            ]);

        var customField1 = CustomFieldFactory.CreateMultiSelect(setup: customFieldSetup);
        customField1.AddOption(option);
        var customField2 = CustomFieldFactory.CreateMultiSelect(setup: customFieldSetup);
        var customFields = new List<MultiSelectCustomField> { customField1, customField2 };
        _projectTaskRepository
            .GetMultiSelectCustomFieldsBySetup(customFieldSetup)
            .Returns(customFields);

        var domainEvent = new RemovedOptionFromMultiSelectCustomFieldSetup(customFieldSetup, option);

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        Utils.CustomFieldSetup.AssertFromRemovedOptionFromMultiSelectCustomFieldSetup(domainEvent, customFields);
    }
}