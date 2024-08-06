using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestData.User.Events;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Events;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.User.Events;

public class UserDeletedHandlerTest
{
    #region Setup

    private readonly IUserRepository _userRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly UserDeletedHandler _uut;

    public UserDeletedHandlerTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new UserDeletedHandler(_userRepository, _workspaceRepository, _projectRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(UserDeletedHandlerData.GetData))]
    public async Task HandleUserDelete_WhenSuccessful_ShouldRemoveDataRelatedToTheUser(
        UserAggregate user,
        List<WorkspaceAggregate> workspaces,
        List<ProjectAggregate> projects)
    {
        // Arrange
        _workspaceRepository
            .GetListByOwner(Arg.Any<UserId>())
            .Returns(workspaces);

        var removedWorkspaces = new List<WorkspaceAggregate>();
        _workspaceRepository
            .When(wr => wr.Remove(Arg.Any<WorkspaceAggregate>()))
            .Do(info => removedWorkspaces.Add(info.Arg<WorkspaceAggregate>()));

        var ownedWorkspaces = workspaces
            .Where(w => w.Memberships.Count > 1)
            .ToList();

        _projectRepository
            .GetListByOwner(Arg.Any<UserId>())
            .Returns(projects);

        var removedProjects = new List<ProjectAggregate>();
        _projectRepository
            .When(wr => wr.Remove(Arg.Any<ProjectAggregate>()))
            .Do(info => removedProjects.Add(info.Arg<ProjectAggregate>()));

        var ownedProjects = projects
            .Where(p => p.Workspace.Memberships.Count > 1 && (!p.IsPrivate.Value || p.AllowedMembers.Count > 1))
            .ToList();

        var domainEvent = new UserDeleted(user);

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetListByOwner(Arg.Is<UserId>(uId => uId == user.Id));
        
        var emptyWorkspaces = workspaces
            .Where(w => w.Memberships.Count == 1)
            .ToList();
        _workspaceRepository
            .Received(emptyWorkspaces.Count)
            .Remove(Arg.Any<WorkspaceAggregate>());
        removedWorkspaces.Should().BeEquivalentTo(emptyWorkspaces);
        
        await _projectRepository
            .Received(1)
            .GetListByOwner(Arg.Is<UserId>(uId => uId == user.Id));

        var emptyProjects = projects
            .Where(p => p.AllowedMembers.Count == 1 && p.IsPrivate.Value && p.Workspace.Memberships.Count > 1)
            .ToList();
        _projectRepository
            .Received(emptyProjects.Count)
            .Remove(Arg.Any<ProjectAggregate>());
        removedProjects.Should().BeEquivalentTo(emptyProjects);

        await _userRepository
            .Received(1)
            .NullifyAuditsByUser(user);
        await _userRepository
            .Received(1)
            .RemoveWorkspaceInvitationsByUser(user);
        
        Utils.User.AssertFromUserDeleted(user, workspaces, projects, ownedWorkspaces, ownedProjects);
    }
}