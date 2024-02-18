using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries;

public class GetWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly GetWorkspaceQueryHandler _uut;

    public GetWorkspaceQueryHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new GetWorkspaceQueryHandler(_workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetWorkspaceQuery_WhenIsSuccessful_ShouldReturnWorkspace()
    {
        // Arrange
        var query = new GetWorkspaceQuery(new Guid("00000000-0000-0000-0000-000000000000"));

        var workspace = Mock.Mock.Workspace.CreateMock();
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
    public async Task HandleGetWorkspaceQuery_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = new GetWorkspaceQuery(new Guid("00000000-0000-0000-0000-000000000000"));

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(id => query.Id == id.Value));

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}