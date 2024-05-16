using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Sprint.Queries.GetCollection;

public class GetCollectionProjectSprintQueryHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly GetCollectionProjectSprintQueryHandler _uut;

    public GetCollectionProjectSprintQueryHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new GetCollectionProjectSprintQueryHandler(_projectSprintRepository);
    }
    
    #endregion

    [Fact]
    public async Task HandleGetCollectionProjectSprintQuery_WhenSuccessful_ShouldReturnProjectSprints()
    {
        // Arrange
        var query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery();

        var sprints = ProjectSprintFactory.CreateList();
        _projectSprintRepository
            .GetListByProject(Arg.Any<ProjectId>())
            .Returns(sprints);
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetListByProject(Arg.Is<ProjectId>(pId => pId.Value == query.ProjectId));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Sprints.Should().BeEquivalentTo(sprints);
    }
}