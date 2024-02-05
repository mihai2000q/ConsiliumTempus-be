using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Commands.Create;

public class WorkspaceCreateCommandHandlerTest
{
    #region Setup

    private readonly Mock<ISecurity> _security;
    private readonly Mock<IWorkspaceRepository> _workspaceRepository;
    private readonly WorkspaceCreateCommandHandler _uut;

    public WorkspaceCreateCommandHandlerTest()
    {
        _security = new Mock<ISecurity>();
        _workspaceRepository = new Mock<IWorkspaceRepository>();
        _uut = new WorkspaceCreateCommandHandler(_security.Object, _workspaceRepository.Object);
    }

    #endregion

    [Fact]
    public async Task WhenWorkspaceCreateIsSuccessful_ShouldAddUserAndReturnNewWorkspace()
    {
        // Arrange
        var command = new WorkspaceCreateCommand(
            "Workspace 1",
            "This is a description",
            "This is a token");

        var user = Mock.Mock.User.CreateMock();
        _security.Setup(s => s.GetUserFromToken(command.Token))
            .ReturnsAsync(user);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _security.Verify(s => s.GetUserFromToken(It.IsAny<string>()), Times.Once());
        _workspaceRepository.Verify(w => w.Add(
                It.Is<WorkspaceAggregate>(workspace =>
                    Utils.Workspace.AssertFromCreateCommand(workspace, command, user))),
            Times.Once());

        outcome.IsError.Should().BeFalse();
        Utils.Workspace.AssertFromCreateCommand(outcome.Value.Workspace, command, user);
    }
    
    [Fact]
    public async Task WhenWorkspaceCreateFailsDueToToken_ShouldReturnInvalidToken()
    {
        // Arrange
        var command = new WorkspaceCreateCommand(
            "Workspace 1",
            "This is a description",
            "This is a token");

        var error = Errors.Authentication.InvalidToken;
        _security.Setup(s => s.GetUserFromToken(command.Token))
            .ReturnsAsync(error);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        _security.Verify(s => s.GetUserFromToken(It.IsAny<string>()), Times.Once());
        _workspaceRepository.Verify(w => 
            w.Add(It.IsAny<WorkspaceAggregate>()), Times.Never);

        outcome.IsError.Should().BeTrue();
        outcome.Errors.Should().HaveCount(1);
        outcome.FirstError.Should().Be(error);
    }
}