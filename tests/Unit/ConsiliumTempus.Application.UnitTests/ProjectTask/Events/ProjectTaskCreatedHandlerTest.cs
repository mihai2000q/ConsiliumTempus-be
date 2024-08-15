using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Events;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.Events;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Events;

public class ProjectTaskCreatedHandlerTest
{
    #region Setup

    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly ProjectTaskCreatedHandler _uut;

    public ProjectTaskCreatedHandlerTest()
    {
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _uut = new ProjectTaskCreatedHandler(_customFieldSetupRepository);
    }

    #endregion

    [Fact]
    public async Task HandleProjectTaskCreated_WhenSuccessful_ShouldCreateCustomFieldsOnProjectFromSetups()
    {
        // Arrange
        var domainEvent = new ProjectTaskCreated(ProjectTaskFactory.Create());

        var customFieldSetups = CustomFieldSetupFactory.CreateList();
        customFieldSetups.Add(CustomFieldSetupFactory.CreateNumber());
        customFieldSetups.Add(CustomFieldSetupFactory.CreateSingleSelect());
        customFieldSetups.Add(CustomFieldSetupFactory.CreateText());
        _customFieldSetupRepository
            .GetList(Arg.Any<WorkspaceId?>(), Arg.Any<ProjectId?>())
            .Returns(customFieldSetups);

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        await _customFieldSetupRepository
            .Received(1)
            .GetList(
                Arg.Is<WorkspaceId?>(wId => wId == null),
                Arg.Is<ProjectId?>(pId => pId == domainEvent.ProjectTask.Stage.Sprint.Project.Id));

        Utils.ProjectTask.AssertFromProjectTaskCreated(domainEvent, customFieldSetups);
    }
}