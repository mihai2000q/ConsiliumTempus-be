using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.CustomFieldSetup.Events;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;

namespace ConsiliumTempus.Application.UnitTests.CustomFieldSetup.Events;

public class RemovedCustomFieldSetupToProjectHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly RemovedCustomFieldSetupFromProjectHandler _uut;

    public RemovedCustomFieldSetupToProjectHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new RemovedCustomFieldSetupFromProjectHandler(_projectTaskRepository);
    }

    #endregion

    [Fact]
    public async Task 
        HandleRemovedCustomFieldSetupFromProject_WhenSuccessful_ShouldDeleteCustomFieldsFromProjectTasksByProjectAndSetup()
    {
        // Arrange
        var domainEvent = new RemovedCustomFieldSetupFromProject(
            CustomFieldSetupFactory.CreateText(),
            ProjectFactory.Create());

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .DeleteCustomFieldsByProjectAndSetup(
                Arg.Is(domainEvent.CustomFieldSetup),
                Arg.Is(domainEvent.Project));
    }
}