using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Events;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Events;

namespace ConsiliumTempus.Application.UnitTests.Project.Events;

public class ProjectDeletedHandlerTest
{
    #region Setup

    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly ProjectDeletedHandler _uut;

    public ProjectDeletedHandlerTest()
    {
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new ProjectDeletedHandler(_customFieldSetupRepository, _projectTaskRepository);
    }

    #endregion

    [Fact]
    public async Task HandleProjectDeleted_WhenSuccessful_ShouldDeleteRelatedCustomFieldsAndCustomFieldSetups()
    {
        // Arrange
        var domainEvent = new ProjectDeleted(ProjectFactory.Create());

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .DeleteCustomFieldsByProject(Arg.Is<ProjectAggregate>(p => p == domainEvent.Project));
        await _customFieldSetupRepository
            .Received(1)
            .DeleteByProject(Arg.Is<ProjectAggregate>(p => p == domainEvent.Project));
    }
}