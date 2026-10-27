using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Workspace.Events;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Workspace.Events;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Events;

public class WorkspaceDeletedHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly ICustomFieldSetupRepository _customFieldSetupRepository;
    private readonly WorkspaceDeletedHandler _uut;

    public WorkspaceDeletedHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _customFieldSetupRepository = Substitute.For<ICustomFieldSetupRepository>();
        _uut = new WorkspaceDeletedHandler(_projectRepository, _customFieldSetupRepository);
    }

    #endregion

    [Fact]
    public async Task
        HandleWorkspaceDeleted_WhenSuccessful_ShouldRemoveCustomFieldSetupsRelatedToWorkspaceOrProjects()
    {
        // Arrange
        var domainEvent = new WorkspaceDeleted(WorkspaceFactory.Create());

        var projects = ProjectFactory.CreateList();
        _projectRepository
            .GetListByWorkspace(Arg.Any<WorkspaceId>())
            .Returns(projects);

        // Act
        await _uut.Handle(domainEvent, CancellationToken.None);

        // Assert
        await _projectRepository
            .Received(1)
            .GetListByWorkspace(Arg.Is(domainEvent.Workspace.Id));

        await _customFieldSetupRepository
            .Received(1)
            .DeleteByWorkspaceOrProjects(Arg.Is(domainEvent.Workspace), Arg.Is(projects));
    }
}