using ConsiliumTempus.Application.Authentication.Events;
using ConsiliumTempus.Domain.User.Events;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Events;

public class UserRegisteredHandlerTest
{
    #region Setup

    private readonly UserRegisteredHandler _uut;

    public UserRegisteredHandlerTest()
    {
        _uut = new UserRegisteredHandler();
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
        user.Workspaces.Should().HaveCount(1);
        user.Workspaces[0].Id.Should().NotBeNull();
        user.Workspaces[0].Name.Should().Be("My Workspace");
        user.Workspaces[0].Description.Should().Be("This is your workspace, where you can create anything you desire. " +
                                            "Take a quick look on our features");
    }
}