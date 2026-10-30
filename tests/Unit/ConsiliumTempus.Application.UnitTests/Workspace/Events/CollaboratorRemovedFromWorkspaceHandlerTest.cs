using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Events;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Events;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
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
            favoriteOnlyProjects,
            projectsToRemove);
    }

    [Fact]
    public async Task
        HandleCollaboratorRemovedFromWorkspace_WhenCollaboratorOwnsProject_ShouldMakeTheNewOwnerAnAllowedMember()
    {
        // Arrange
        var workspace = WorkspaceFactory.Create();
        var nextOwner = UserFactory.Create();
        workspace.AddUserMembership(MembershipFactory.Create(nextOwner, workspaceRole: WorkspaceRole.Admin));
        var collaboratorMembership = MembershipFactory.Create(workspaceRole: WorkspaceRole.View);
        workspace.AddUserMembership(collaboratorMembership);
        var collaborator = collaboratorMembership.User;
        workspace.RemoveUserMembership(collaboratorMembership);
        workspace.ClearDomainEvents();

        var otherAllowedMember = UserFactory.Create();
        var privateProject = ProjectFactory.Create(isPrivate: true, owner: collaborator, workspace: workspace);
        privateProject.AddAllowedMember(otherAllowedMember);
        var publicProject = ProjectFactory.Create(isPrivate: false, owner: collaborator, workspace: workspace);
        List<ProjectAggregate> projects = [privateProject, publicProject];
        _projectRepository
            .GetRelatedListByUserAndWorkspace(Arg.Any<UserId>(), Arg.Any<WorkspaceId>())
            .Returns(projects);

        // Act
        await _uut.Handle(new CollaboratorRemovedFromWorkspace(workspace, collaborator), CancellationToken.None);

        // Assert
        _projectRepository.DidNotReceive().Remove(Arg.Any<ProjectAggregate>());

        projects.Should().AllSatisfy(p =>
        {
            p.Owner.Should().Be(nextOwner);
            p.AllowedMembers.Should().Contain(nextOwner);
            p.AllowedMembers.Should().NotContain(collaborator);
            p.AllowedMembers.Count(u => u == nextOwner).Should().Be(1);
        });
        privateProject.AllowedMembers.Should().Contain(otherAllowedMember);
    }

    [Fact]
    public async Task
        HandleCollaboratorRemovedFromWorkspace_WhenTheNewOwnerIsAlreadyAllowedMember_ShouldNotAddThemAgain()
    {
        // Arrange
        var workspace = WorkspaceFactory.Create();
        var nextOwner = UserFactory.Create();
        workspace.AddUserMembership(MembershipFactory.Create(nextOwner, workspaceRole: WorkspaceRole.Admin));
        var collaboratorMembership = MembershipFactory.Create(workspaceRole: WorkspaceRole.View);
        workspace.AddUserMembership(collaboratorMembership);
        var collaborator = collaboratorMembership.User;
        workspace.RemoveUserMembership(collaboratorMembership);
        workspace.ClearDomainEvents();

        var project = ProjectFactory.Create(isPrivate: true, owner: collaborator, workspace: workspace);
        project.AddAllowedMember(nextOwner);
        _projectRepository
            .GetRelatedListByUserAndWorkspace(Arg.Any<UserId>(), Arg.Any<WorkspaceId>())
            .Returns([project]);

        // Act
        await _uut.Handle(new CollaboratorRemovedFromWorkspace(workspace, collaborator), CancellationToken.None);

        // Assert
        project.Owner.Should().Be(nextOwner);
        project.AllowedMembers.Count(u => u == nextOwner).Should().Be(1);
        project.AllowedMembers.Should().NotContain(collaborator);
    }
}