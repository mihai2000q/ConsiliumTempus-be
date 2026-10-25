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

        var previousOwners = projects.ToDictionary(p => p, p => p.Owner);

        var projectsToRemove = projects
            .Where(p =>
                p.IsPrivate.Value &&
                p.AllowedMembers.Count == 1 &&
                p.AllowedMembers.Contains(domainEvent.Collaborator))
            .ToList();

        var allowedMemberProjects = projects
            .Where(p => !projectsToRemove.Contains(p) && p.AllowedMembers.Contains(domainEvent.Collaborator))
            .ToList();

        var favoriteOnlyProjects = projects
            .Where(p => p.IsFavorite(domainEvent.Collaborator) && !p.AllowedMembers.Contains(domainEvent.Collaborator))
            .ToList();

        // Act
        await _uut.Handle(domainEvent, CancellationToken.None);

        // Assert
        await _projectRepository
            .Received(1)
            .GetRelatedListByUserAndWorkspace(
                Arg.Is(domainEvent.Collaborator.Id),
                Arg.Is(domainEvent.Workspace.Id));

        _projectRepository
            .Received(projectsToRemove.Count)
            .Remove(Arg.Any<ProjectAggregate>());
        projectsToRemove.ForEach(project =>
            _projectRepository
                .Received(1)
                .Remove(project));

        Utils.Workspace.AssertFromRemovedCollaborator(
            domainEvent,
            projects.Where(p => !projectsToRemove.Contains(p)).ToList(),
            previousOwners,
            allowedMemberProjects,
            favoriteOnlyProjects);
    }
}