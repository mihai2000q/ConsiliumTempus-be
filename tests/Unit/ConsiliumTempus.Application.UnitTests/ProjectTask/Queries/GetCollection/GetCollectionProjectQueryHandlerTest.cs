using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Queries.GetCollection;

public class GetCollectionProjectTaskQueryHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly GetCollectionProjectTaskQueryHandler _uut;

    public GetCollectionProjectTaskQueryHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new GetCollectionProjectTaskQueryHandler(_projectTaskRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetCollectionProjectTaskQuery_WhenSucceeds_ShouldReturnProjectTasks()
    {
        // Arrange
        var query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery();
        
        var tasks = ProjectTaskFactory.CreateList();
        _projectTaskRepository
            .GetListByStage(
                Arg.Any<ProjectStageId>(),
                Arg.Any<IReadOnlyList<IFilter<ProjectTaskAggregate>>>())
            .Returns(tasks);

        const int projectTasksCount = 25;
        _projectTaskRepository
            .GetListByStageCount(
                Arg.Any<ProjectStageId>(),
                Arg.Any<IReadOnlyList<IFilter<ProjectTaskAggregate>>>())
            .Returns(projectTasksCount);
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .GetListByStage(
                Arg.Is<ProjectStageId>(sId => sId.Value == query.ProjectStageId),
                Arg.Is<IReadOnlyList<IFilter<ProjectTaskAggregate>>>(filters =>
                    filters.AssertFilters(query.Search, ProjectTaskFilter.FilterProperties)));

        await _projectTaskRepository
            .GetListByStageCount(
                Arg.Is<ProjectStageId>(sId => sId.Value == query.ProjectStageId),
                Arg.Is<IReadOnlyList<IFilter<ProjectTaskAggregate>>>(filters =>
                    filters.AssertFilters(query.Search, ProjectTaskFilter.FilterProperties)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Tasks.Should().BeEquivalentTo(tasks);
        outcome.Value.TotalCount.Should().Be(projectTasksCount);
    }
}