using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.CustomFieldSetup.Events;
using ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Events;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Events;

public class AddedCustomFieldSetupToProjectHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly AddedCustomFieldSetupToProjectHandler _uut;

    public AddedCustomFieldSetupToProjectHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new AddedCustomFieldSetupToProjectHandler(_projectTaskRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(AddedCustomFieldSetupToProjectHandlerData.GetCustomFieldSetups))]
    public async Task HandleAddedCustomFieldSetupToProject_WhenSuccessful_ShouldCreateCustomFieldOnAllProjectTasks(
        CustomFieldSetupAggregate setup)
    {
        // Arrange
        var domainEvent = new AddedCustomFieldSetupToProject(setup, ProjectFactory.Create());

        var tasks = ProjectTaskFactory.CreateList();
        _projectTaskRepository
            .GetListByProject(Arg.Any<ProjectId>())
            .Returns(tasks);

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetListByProject(Arg.Is<ProjectId>(pId => pId == domainEvent.Project.Id));

        Utils.CustomFieldSetup.AssertFromAddedCustomFieldSetupToProject(domainEvent, tasks);
    }
}