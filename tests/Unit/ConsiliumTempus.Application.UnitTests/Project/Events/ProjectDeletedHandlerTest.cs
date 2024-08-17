using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Events;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.Project.Events;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Project.Events;

public class ProjectDeletedHandlerTest
{
    #region Setup

    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly ProjectDeletedHandler _uut;

    public ProjectDeletedHandlerTest()
    {
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _uut = new ProjectDeletedHandler(_customFieldSetupRepository);
    }

    #endregion

    [Fact]
    public async Task HandleProjectDeleted_WhenSuccessful_ShouldDeletedRelatedCustomFieldSetups()
    {
        // Arrange
        var domainEvent = new ProjectDeleted(ProjectFactory.Create());

        var customFieldSetups = CustomFieldSetupFactory.CreateList();
        _customFieldSetupRepository
            .GetListByProject(Arg.Any<ProjectId>())
            .Returns(customFieldSetups);

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        await _customFieldSetupRepository
            .Received(1)
            .GetListByProject(Arg.Is<ProjectId>(pId => pId == domainEvent.Project.Id));
        _customFieldSetupRepository
            .Received(1)
            .RemoveRange(Arg.Is<List<CustomFieldSetupAggregate>>(cfs => cfs == customFieldSetups));
    }
}