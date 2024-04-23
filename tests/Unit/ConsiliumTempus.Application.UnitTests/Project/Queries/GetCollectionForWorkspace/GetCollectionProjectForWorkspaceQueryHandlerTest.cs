using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.GetCollectionForWorkspace;

public class GetCollectionProjectForWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly GetCollectionProjectForWorkspaceQueryHandler _uut;

    public GetCollectionProjectForWorkspaceQueryHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new GetCollectionProjectForWorkspaceQueryHandler(_workspaceRepository, _projectRepository);
    }

    #endregion

    [Fact]
    public async Task GetCollectionProjectForWorkspace_WhenWorkspaceIsNull_ShouldReturnWorkspaceNotFoundError()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetCollectionProjectForWorkspaceQuery();

        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .ReturnsNull();
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(wId => wId.Value == query.WorkspaceId));
        _projectRepository.DidNotReceive();
        
        outcome.ValidateError(Errors.Workspace.NotFound);
    }
    
    [Fact]
    public async Task GetCollectionProjectForWorkspace_WhenSucceeds_ShouldReturnProjects()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetCollectionProjectForWorkspaceQuery();

        var workspace = WorkspaceFactory.Create();
        _workspaceRepository
            .Get(Arg.Any<WorkspaceId>())
            .Returns(workspace);

        var projects = ProjectFactory.CreateList();
        _projectRepository
            .GetListByWorkspace(Arg.Any<WorkspaceId>(), Arg.Any<IReadOnlyList<IFilter<ProjectAggregate>>>())
            .Returns(projects);
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .Get(Arg.Is<WorkspaceId>(wId => wId.Value == query.WorkspaceId));
        await _projectRepository
            .Received(1)
            .GetListByWorkspace(
                Arg.Is<WorkspaceId>(wId => wId == workspace.Id), 
                Arg.Is<IReadOnlyList<IFilter<ProjectAggregate>>>(filters => 
                    Utils.Project.AssertGetCollectionProjectForWorkspaceFilters(filters, query)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Projects.Should().BeEquivalentTo(projects);
    }
}