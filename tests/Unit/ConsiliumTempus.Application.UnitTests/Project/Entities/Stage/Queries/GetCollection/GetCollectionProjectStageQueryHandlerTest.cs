using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Entities.Stage.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Stage.Queries.GetCollection;

public class GetCollectionProjectStageQueryHandlerTest
{
    #region Setup

    private readonly IProjectStageRepository _projectStageRepository;
    private readonly GetCollectionProjectStageQueryHandler _uut;

    public GetCollectionProjectStageQueryHandlerTest()
    {
        _projectStageRepository = Substitute.For<IProjectStageRepository>();
        _uut = new GetCollectionProjectStageQueryHandler(_projectStageRepository);
    }
    
    #endregion

    [Fact]
    public async Task HandleGetCollectionProjectStageQuery_WhenSuccessful_ShouldReturnProjectStages()
    {
        // Arrange
        var query = ProjectStageQueryFactory.CreateGetCollectionProjectStageQuery();

        var stages = ProjectStageFactory.CreateList();
        _projectStageRepository
            .GetListBySprint(Arg.Any<ProjectSprintId>())
            .Returns(stages);
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectStageRepository
            .Received(1)
            .GetListBySprint(Arg.Is<ProjectSprintId>(pId => pId.Value == query.ProjectSprintId));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Stages.Should().BeEquivalentTo(stages);
    }
    
}