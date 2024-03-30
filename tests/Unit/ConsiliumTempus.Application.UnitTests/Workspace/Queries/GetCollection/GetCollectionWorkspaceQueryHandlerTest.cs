using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.GetCollection;

public class GetCollectionWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly GetCollectionWorkspaceQueryHandler _uut;

    public GetCollectionWorkspaceQueryHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new GetCollectionWorkspaceQueryHandler(_currentUserProvider, _workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task WhenGetCollectionWorkspaceIsSuccessful_ShouldReturnCollectionOfWorkspaces()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery();

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUser()
            .Returns(user);

        var workspaces = WorkspaceFactory.CreateList();
        _workspaceRepository
            .GetListForUser(user)
            .Returns(workspaces);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();
        await _workspaceRepository
            .Received(1)
            .GetListForUser(Arg.Any<UserAggregate>());
        
        outcome.Value.Should().BeEquivalentTo(workspaces);
    }
    
    [Fact]
    public async Task WhenGetCollectionWorkspaceFails_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();
        _workspaceRepository.DidNotReceive();
        
        outcome.ValidateError(Errors.User.NotFound);
    }
}