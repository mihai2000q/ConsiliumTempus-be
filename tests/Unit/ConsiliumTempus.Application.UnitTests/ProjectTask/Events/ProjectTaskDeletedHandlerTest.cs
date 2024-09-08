using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Events;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Events;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Events;

public class ProjectTaskDeletedHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly ProjectTaskDeletedHandler _uut;

    public ProjectTaskDeletedHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new ProjectTaskDeletedHandler(_projectTaskRepository);
    }

    #endregion

    [Fact]
    public async Task HandleProjectTaskDeleted_WhenSuccessful_ShouldDeleteCustomFieldsByTask()
    {
        // Arrange
        var domainEvent = new ProjectTaskDeleted(ProjectTaskFactory.Create());

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .DeleteCustomFieldsByTask(Arg.Is<ProjectTaskId>(id => id == domainEvent.ProjectTask.Id));
    }
}