using ConsiliumTempus.Application.Authentication.Events;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.Authentication.Events;

public class UserRegisteredHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UserRegisteredHandler _uut;

    public UserRegisteredHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new UserRegisteredHandler(_workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUserRegistered_WhenSuccessful_ShouldAddWorkspaceToUser()
    {
        // Arrange
        var user = UserFactory.Create();

        // Act
        await _uut.Handle(new UserRegistered(user), default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Add(Arg.Is<WorkspaceAggregate>(workspace =>
                Utils.User.AssertFromUserRegistered(user, workspace)));
    }
}