using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.GetCollaborators;

public class GetCollaboratorsFromWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly GetCollaboratorsFromWorkspaceQueryHandler _uut;

    public GetCollaboratorsFromWorkspaceQueryHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new GetCollaboratorsFromWorkspaceQueryHandler(_workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetCollaboratorsFromWorkspaceQuery_WhenIsSuccessful_ShouldReturnCollaborators()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery();

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .GetWithMemberships(Arg.Any<WorkspaceId>(), Arg.Any<string>())
            .Returns(workspace);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithMemberships(
                Arg.Is<WorkspaceId>(id => query.Id == id.Value),
                Arg.Is<string>(s => s == query.SearchValue));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Collaborators.Should().BeEquivalentTo(workspace.Memberships.Select(m => m.User));
    }

    [Fact]
    public async Task HandleGetCollaboratorsFromWorkspaceQuery_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery();

        _workspaceRepository
            .GetWithMemberships(Arg.Any<WorkspaceId>(), Arg.Any<string>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetWithMemberships(
                Arg.Is<WorkspaceId>(id => query.Id == id.Value),
                Arg.Is<string>(s => s == query.SearchValue));

        outcome.ValidateError(Errors.Workspace.NotFound);
    }
}