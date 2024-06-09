using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.Get;

public class GetWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly GetWorkspaceQueryHandler _uut;

    public GetWorkspaceQueryHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new GetWorkspaceQueryHandler(_currentUserProvider, _workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetWorkspaceQuery_WhenIsSuccessful_ShouldReturnWorkspace()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetWorkspaceQuery();

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => query.Id == id.Value));

        outcome.IsError.Should().BeFalse();
        Utils.Workspace.AssertWorkspace(outcome.Value, workspace, user);
    }

    [Fact]
    public async Task HandleGetWorkspaceQuery_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetWorkspaceQuery();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => query.Id == id.Value));

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}