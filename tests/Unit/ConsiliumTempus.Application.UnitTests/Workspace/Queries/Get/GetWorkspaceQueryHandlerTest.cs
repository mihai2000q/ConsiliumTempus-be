using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.Get;

public class GetWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly Mock<IWorkspaceRepository> _workspaceRepository;
    private readonly GetWorkspaceQueryHandler _uut;

    public GetWorkspaceQueryHandlerTest()
    {
        _workspaceRepository = new Mock<IWorkspaceRepository>();
        _uut = new GetWorkspaceQueryHandler(_workspaceRepository.Object);
    }

    #endregion

    [Fact]
    public async Task HandleGetWorkspaceQuery_WhenIsSuccessful_ShouldReturnWorkspace()
    {
        // Arrange
        var query = new GetWorkspaceQuery("00000000-0000-0000-0000-000000000000");

        var workspace = Mock.Mock.Workspace.CreateMock();
        _workspaceRepository.Setup(w =>
                w.Get(It.IsAny<WorkspaceId>(), default))
            .ReturnsAsync(workspace);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        _workspaceRepository.Verify(w =>
                w.Get(It.Is<WorkspaceId>(i => 
                    Utils.Workspace.AssertWorkspaceId(i, query.Id)), default),
            Times.Once());

        outcome.IsError.Should().BeFalse();
        Utils.Workspace.AssertGetResult(outcome.Value, workspace);
    }

    [Fact]
    public async Task HandleGetWorkspaceQuery_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = new GetWorkspaceQuery("00000000-0000-0000-0000-000000000000");

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        _workspaceRepository.Verify(w =>
                w.Get(It.Is<WorkspaceId>(i =>
                    Utils.Workspace.AssertWorkspaceId(i, query.Id)), default),
            Times.Once());

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}