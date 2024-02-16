using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries;

public class GetCollectionWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly Mock<ISecurity> _security;
    private readonly Mock<IWorkspaceRepository> _workspaceRepository;
    private readonly GetCollectionWorkspaceQueryHandler _uut;

    public GetCollectionWorkspaceQueryHandlerTest()
    {
        _security = new Mock<ISecurity>();
        _workspaceRepository = new Mock<IWorkspaceRepository>();
        _uut = new GetCollectionWorkspaceQueryHandler(_workspaceRepository.Object, _security.Object);
    }

    #endregion

    [Fact]
    public async Task WhenGetCollectionWorkspace_ShouldReturnCollectionOfWorkspaces()
    {
        // Arrange
        var query = new GetCollectionWorkspaceQuery("This is a token");

        var user = Mock.Mock.User.CreateMock();
        _security.Setup(s => s.GetUserFromToken(query.Token, default))
            .ReturnsAsync(user);

        var workspaces = Mock.Mock.Workspace.CreateListMock();
        _workspaceRepository.Setup(w => w.GetListForUser(user, default))
            .ReturnsAsync(workspaces);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        outcome.Should().BeEquivalentTo(workspaces);
    }
}