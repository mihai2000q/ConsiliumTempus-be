using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint;
using NSubstitute.ReturnsExtensions;

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
                Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>())
            .Returns(sprints);

        const int totalCount = 25;
        _projectSprintRepository
            .GetListByProjectCount(
                Arg.Any<ProjectId>(),
                Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>())
            .Returns(totalCount);

        // Act
        var outcome = await _uut.Handle(query, CancellationToken.None);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetListByProject(
                Arg.Is<ProjectId>(pId => pId.Value == query.ProjectId),
                Arg.Is<IReadOnlyList<IFilter<ProjectSprintAggregate>>>(filters =>
                    Utils.ProjectSprint.AssertGetCollectionFilters(filters, query)));
        await _projectSprintRepository
            .Received(1)
            .GetListByProjectCount(
                Arg.Is<ProjectId>(pId => pId.Value == query.ProjectId),
                Arg.Is<IReadOnlyList<IFilter<ProjectSprintAggregate>>>(filters =>
                    Utils.ProjectSprint.AssertGetCollectionFilters(filters, query)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Sprints.Should().BeEquivalentTo(sprints);
        outcome.Value.TotalCount.Should().Be(totalCount);

        await _projectSprintRepository
            .DidNotReceive()
            .GetFirstByProject(
                Arg.Any<ProjectId>(),
                Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>());
    }

    [Fact]
    public async Task
        HandleGetCollectionProjectSprintQuery_WhenThereAreNoSprintsFromThisYearAndNoFilters_ShouldReturnFirstSprint()
    {
        // Arrange
        var query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(fromThisYear: true);

        _projectSprintRepository
            .GetListByProject(
                Arg.Any<ProjectId>(),
                Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>())
            .Returns([]);

        _projectSprintRepository
            .GetListByProjectCount(
                Arg.Any<ProjectId>(),
                Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>())
            .Returns(0);

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
                    Utils.ProjectSprint.AssertGetCollectionFilters(filters, query)));
        await _projectSprintRepository
            .Received(1)
            .GetListByProjectCount(
                Arg.Is<ProjectId>(pId => pId.Value == query.ProjectId),
                Arg.Is<IReadOnlyList<IFilter<ProjectSprintAggregate>>>(filters =>
                    Utils.ProjectSprint.AssertGetCollectionFilters(filters, query)));
        await _projectSprintRepository
            .Received(1)
            .GetFirstByProject(
                Arg.Is<ProjectId>(pId => pId.Value == query.ProjectId),
                Arg.Is<IReadOnlyList<IFilter<ProjectSprintAggregate>>>(filters =>
                    filters.AssertFilters(query.Search, ProjectSprintFilter.FilterProperties)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Sprints.Should().HaveCount(1);
        outcome.Value.Sprints[0].Should().Be(sprint);
        outcome.Value.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task
        HandleGetCollectionProjectSprintQuery_WhenThereAreNoSprintsAtAll_ShouldReturnNoSprints()
    {
        // Arrange
        var query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(fromThisYear: true);

        _projectSprintRepository
            .GetListByProject(
                Arg.Any<ProjectId>(),
                Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>())
            .Returns([]);
        _projectSprintRepository
            .GetListByProjectCount(
                Arg.Any<ProjectId>(),
                Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>())
            .Returns(0);
        _projectSprintRepository
            .GetFirstByProject(
                Arg.Any<ProjectId>(),
                Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetFirstByProject(
                Arg.Is<ProjectId>(pId => pId.Value == query.ProjectId),
                Arg.Any<IReadOnlyList<IFilter<ProjectSprintAggregate>>>());

        outcome.IsError.Should().BeFalse();
        outcome.Value.Sprints.Should().BeEmpty();
        outcome.Value.TotalCount.Should().Be(0);
    }
}