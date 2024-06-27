using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Queries.GetOverview;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.GetOverview;

public class GetOverviewWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly GetOverviewWorkspaceQueryHandler _uut;

    public GetOverviewWorkspaceQueryHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new GetOverviewWorkspaceQueryHandler(_workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetOverviewWorkspaceQuery_WhenIsSuccessful_ShouldReturnWorkspace()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetOverviewWorkspaceQuery();

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => query.Id == id.Value));

        outcome.IsError.Should().BeFalse();
        Utils.Workspace.AssertWorkspace(outcome.Value, workspace);
    }

    [Fact]
    public async Task HandleGetOverviewWorkspaceQuery_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetOverviewWorkspaceQuery();

        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => query.Id == id.Value));

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}