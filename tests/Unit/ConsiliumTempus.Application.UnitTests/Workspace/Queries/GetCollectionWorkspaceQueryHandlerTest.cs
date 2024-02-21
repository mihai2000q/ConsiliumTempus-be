using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries;

public class GetCollectionWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly ISecurity _security;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly GetCollectionWorkspaceQueryHandler _uut;

    public GetCollectionWorkspaceQueryHandlerTest()
    {
        _security = Substitute.For<ISecurity>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new GetCollectionWorkspaceQueryHandler(_workspaceRepository, _security);
    }

    #endregion

    [Fact]
    public async Task WhenGetCollectionWorkspace_ShouldReturnCollectionOfWorkspaces()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery();

        var user = UserFactory.Create();
        _security
            .GetUserFromToken(query.Token)
            .Returns(user);

        var workspaces = WorkspaceFactory.CreateList();
        _workspaceRepository
            .GetListForUser(user)
            .Returns(workspaces);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _security
            .Received(1)
            .GetUserFromToken(Arg.Any<string>());
        await _workspaceRepository
            .Received(1)
            .GetListForUser(Arg.Any<UserAggregate>());
        
        outcome.Should().BeEquivalentTo(workspaces);
    }
}