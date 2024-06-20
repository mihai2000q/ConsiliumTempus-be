using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

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

        var collaborators = UserFactory.CreateList();
        _workspaceRepository
            .GetCollaborators(Arg.Any<WorkspaceId>(), Arg.Any<string>())
            .Returns(collaborators);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetCollaborators(
                Arg.Is<WorkspaceId>(id => query.Id == id.Value),
                Arg.Is<string>(s => s == query.SearchValue));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Collaborators.Should().BeEquivalentTo(collaborators);
    }
}