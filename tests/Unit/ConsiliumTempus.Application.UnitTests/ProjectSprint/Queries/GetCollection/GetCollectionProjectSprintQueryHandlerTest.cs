using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Queries.GetCollection;

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

    [Theory]
    [ClassData(typeof(GetCollectionProjectSprintQueryHandlerData.GetQueriesAndSprints))]
    public async Task HandleGetCollectionProjectSprintQuery_WhenSuccessful_ShouldReturnProjectSprints(
        GetCollectionProjectSprintQuery query,
        List<ProjectSprintAggregate> sprints)
    {
        // Arrange
        _projectSprintRepository
            .GetListByProject(
                Arg.Any<ProjectId>(), 
                Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>(),
                Arg.Any<bool>())
            .Returns(sprints);

        const int totalCount = 25;
        _projectSprintRepository
            .GetListByProjectCount(
                Arg.Any<ProjectId>(), 
                Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>(),
                Arg.Any<bool>())
            .Returns(totalCount);

        var sprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetFirstByProject(
                Arg.Any<ProjectId>(),
                Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>())
            .Returns(sprint);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetListByProject(
                Arg.Is<ProjectId>(pId => pId.Value == query.ProjectId),
                Arg.Is<IReadOnlyList<IFilter<ProjectSprintAggregate>>>(filters => 
                    filters.AssertFilters(query.Search, ProjectSprintFilter.FilterProperties)),
                Arg.Is<bool>(f => f == query.FromThisYear));
        await _projectSprintRepository
            .Received(1)
            .GetListByProjectCount(
                Arg.Is<ProjectId>(pId => pId.Value == query.ProjectId),
                Arg.Is<IReadOnlyList<IFilter<ProjectSprintAggregate>>>(filters => 
                    filters.AssertFilters(query.Search, ProjectSprintFilter.FilterProperties)),
                Arg.Is<bool>(f => f == query.FromThisYear));
        
        outcome.IsError.Should().BeFalse();
        if (sprints.IsEmpty() && query.FromThisYear)
        {
            outcome.Value.Sprints.Should().HaveCount(1);
            outcome.Value.Sprints[0].Should().Be(sprint);
            await _projectSprintRepository
                .Received(1)
                .GetFirstByProject(
                    Arg.Is<ProjectId>(pId => pId.Value == query.ProjectId),
                    Arg.Is<IReadOnlyList<IFilter<ProjectSprintAggregate>>>(filters => 
                        filters.AssertFilters(query.Search, ProjectSprintFilter.FilterProperties)));
        }
        else
        { 
            outcome.Value.Sprints.Should().BeEquivalentTo(sprints);
            await _projectSprintRepository
                .DidNotReceive()
                .GetFirstByProject(
                    Arg.Any<ProjectId>(),
                    Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>());
        }
        outcome.Value.TotalCount.Should().Be(totalCount);
    }
}