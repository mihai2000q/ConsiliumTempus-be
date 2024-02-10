using ConsiliumTempus.Application.Authentication.Events;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User.Events;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Events;

public class UserRegisteredHandlerTest
{
    #region Setup

    private readonly Mock<IWorkspaceRoleRepository> _workspaceRoleRepository;
    private readonly UserRegisteredHandler _uut;

    public UserRegisteredHandlerTest()
    {
        _workspaceRoleRepository = new Mock<IWorkspaceRoleRepository>();
        _uut = new UserRegisteredHandler(_workspaceRoleRepository.Object);
    }

    #endregion

    [Fact]
    public async Task WhenUserRegisters_ShouldAddWorkspaceToUser()
    {
        // Arrange
        var user = Mock.Mock.User.CreateMock();

        // Act
        await _uut.Handle(new UserRegistered(user), default);

        // Assert
        _workspaceRoleRepository.Verify(w => w.Attach(WorkspaceRole.Admin), Times.Once());

        user.Memberships.Should().HaveCount(1);
        Utils.Membership.Assert(
            user.Memberships[0],
            user,
            Constants.Workspace.Name,
            Constants.Workspace.Description);
    }
}