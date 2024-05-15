using ConsiliumTempus.Application.Authentication.Events;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.User.Events;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Events;

public class UserRegisteredHandlerTest
{
    #region Setup

    private readonly UserRegisteredHandler _uut = new();

    #endregion

    [Fact]
    public async Task HandleUserRegistered_WhenSuccessful_ShouldAddWorkspaceToUser()
    {
        // Arrange
        var user = UserFactory.Create();

        // Act
        await _uut.Handle(new UserRegistered(user), default);

        // Assert
        user.Memberships.Should().HaveCount(1);
        Utils.Membership.Assert(
            user.Memberships[0],
            user,
            Constants.Workspace.Name,
            Constants.Workspace.Description);
    }
}