using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Events;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Events;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.Events;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Events;

public class CollaboratorRemovedFromWorkspaceHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly CollaboratorRemovedFromWorkspaceHandler _uut;

    public CollaboratorRemovedFromWorkspaceHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new CollaboratorRemovedFromWorkspaceHandler(_projectRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(CollaboratorRemovedFromWorkspaceHandlerData.GetData))]
    public async Task
        HandleCollaboratorRemovedFromWorkspace_WhenSucceeds_ShouldRemoveUserRelatedDataFromWorkspaceAndRelatedProjects(
            CollaboratorRemovedFromWorkspace domainEvent,
            List<ProjectAggregate> projects)
    {
        // Arrange
        _projectRepository
            .GetRelatedListByUserAndWorkspace(
                Arg.Any<UserId>(),
                Arg.Any<WorkspaceId>())
            .Returns(projects);

        var ownedProjects = projects
            .Where(p => p.Owner == domainEvent.Collaborator)
            .ToList();

        var favoritesProjects = projects
            .Where(p => p.IsFavorite(domainEvent.Collaborator))
            .ToList();

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetRelatedListByUserAndWorkspace(
                Arg.Is(domainEvent.Collaborator.Id),
                Arg.Is(domainEvent.Workspace.Id));

        var projectsToRemove = projects
            .Where(p =>
                p.IsPrivate.Value &&
                p.Owner == domainEvent.Collaborator &&
                p.AllowedMembers.Count == 1)
            .ToList();
        _projectRepository
            .Received(projectsToRemove.Count)
            .Remove(Arg.Any<ProjectAggregate>());
        projectsToRemove.ForEach(project =>
            _projectRepository
                .Received(1)
                .Remove(project));

        projects.RemoveAll(projectsToRemove.Contains);
        Utils.Workspace.AssertFromRemovedCollaborator(domainEvent, projects, ownedProjects, favoritesProjects);
    }
}