using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestData.User.Events;
using ConsiliumTempus.Application.User.Events;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.Workspace;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Application.UnitTests.User.Events;

public class UserDeletedHandlerTest
{
    #region Setup

    private readonly IUserRepository _userRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UserDeletedHandler _uut;

    public UserDeletedHandlerTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new UserDeletedHandler(_userRepository, _workspaceRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(UserDeletedHandlerData.GetData))]
    public async Task HandleUserDelete_WhenSuccessful_ShouldEitherDeleteWorkspaceOrTransferOwnership(
        List<WorkspaceAggregate> workspaces,
        UserAggregate user)
    {
        // Arrange
        _workspaceRepository
            .GetListByUserWithCollaborators(Arg.Any<UserAggregate>())
            .Returns(workspaces);
        
        var removedWorkspaces = new List<WorkspaceAggregate>();
        _workspaceRepository
            .When(wr => wr.Remove(Arg.Any<WorkspaceAggregate>()))
            .Do(info => removedWorkspaces.Add(info.Arg<WorkspaceAggregate>()));

        var ownerWorkspaces = workspaces
            .Where(w => w.Owner == user && w.Memberships.Count > 1)
            .ToList();
        
        // Act
        await _uut.Handle(new UserDeleted(user), default);

        // Assert
        await _userRepository
            .Received(1)
            .NullifyAuditsByUser(user);
        await _userRepository
            .Received(1)
            .RemoveProjectsByOwner(user);
        await _userRepository
            .Received(1)
            .RemoveWorkspaceInvitationsByUser(user);
        
        await _workspaceRepository
            .Received(1)
            .GetListByUserWithCollaborators(Arg.Is<UserAggregate>(u => u == user));

        var emptyWorkspaces = workspaces
            .Where(w => w.Memberships.Count == 1)
            .ToList();
        _workspaceRepository
            .Received(emptyWorkspaces.Count)
            .Remove(Arg.Any<WorkspaceAggregate>());
        removedWorkspaces.Should().BeEquivalentTo(emptyWorkspaces);

        var preservedWorkspaces = workspaces
            .Where(w => w.Memberships.Count > 1)
            .ToList();

        preservedWorkspaces.Should().HaveSameCount(ownerWorkspaces);
        preservedWorkspaces.Select(w => w.Id)
            .Should().BeEquivalentTo(ownerWorkspaces.Select(w => w.Id));
        preservedWorkspaces.Should().AllSatisfy(w =>
        {
            w.Owner.Should().NotBe(user);
            w.IsPersonal.Value.Should().BeFalse();
            
            var oldWorkspace = ownerWorkspaces.Single(x => x.Id == w.Id);
            var newAdminOwner = oldWorkspace.Memberships
                .FirstOrDefault(m => m.WorkspaceRole.Equals(WorkspaceRole.Admin) && m.User != user);
            if (newAdminOwner is not null)
            {
                w.Owner.Should().Be(newAdminOwner.User);
            }
            else
            {
                var oldMembership = oldWorkspace.Memberships.First(m => m.User != user);
                oldMembership.WorkspaceRole.Should().NotBe(WorkspaceRole.Admin);
                
                var newOwner = w.Memberships.Single(m => m.Id == oldMembership.Id);
                newOwner.WorkspaceRole.Should().NotBe(oldMembership.WorkspaceRole);
                newOwner.WorkspaceRole.Should().Be(WorkspaceRole.Admin);
                newOwner.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 15.Seconds());
                w.Owner.Should().Be(oldMembership.User);
            }
        });
    }
}