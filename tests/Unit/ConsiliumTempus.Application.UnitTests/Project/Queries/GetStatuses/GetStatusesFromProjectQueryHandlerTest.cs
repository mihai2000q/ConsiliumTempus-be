using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Queries.GetStatuses;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.GetStatuses;

public class GetStatusesFromProjectQueryHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly GetStatusesFromProjectQueryHandler _uut;

    public GetStatusesFromProjectQueryHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new GetStatusesFromProjectQueryHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetStatusesFromProjectQuery_WhenIsSuccessful_ShouldReturnStatusesFromProject()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetStatusesFromProjectQuery();

        var statuses = ProjectStatusFactory.CreateList();
        _projectRepository
            .GetStatuses(Arg.Any<ProjectId>())
            .Returns(statuses);

        const int totalCount = 25;
        _projectRepository
            .GetStatusesCount(Arg.Any<ProjectId>())
            .Returns(totalCount);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetStatuses(Arg.Is<ProjectId>(id => query.Id == id.Value));

        await _projectRepository
            .Received(1)
            .GetStatusesCount(Arg.Is<ProjectId>(id => query.Id == id.Value));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Statuses.Should().BeEquivalentTo(statuses);
        outcome.Value.TotalCount.Should().Be(totalCount);
    }
}