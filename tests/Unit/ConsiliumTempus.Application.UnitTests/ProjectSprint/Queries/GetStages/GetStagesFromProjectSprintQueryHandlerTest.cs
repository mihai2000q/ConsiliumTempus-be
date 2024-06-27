using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetStages;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.ProjectSprint.Queries.GetStages;

public class GetStagesFromProjectSprintQueryHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly GetStagesFromProjectSprintQueryHandler _uut;

    public GetStagesFromProjectSprintQueryHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new GetStagesFromProjectSprintQueryHandler(_projectSprintRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetStagesFromProjectSprintQuery_WhenIsSuccessful_ShouldReturnStages()
    {
        // Arrange
        var query = ProjectSprintQueryFactory.CreateGetStagesFromProjectSprintQuery();

        var stages = ProjectStageFactory.CreateList();
        _projectSprintRepository
            .GetStages(Arg.Any<ProjectSprintId>())
            .Returns(stages);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetStages(Arg.Is<ProjectSprintId>(id => query.Id == id.Value));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Stages.Should().BeEquivalentTo(stages);
    }
}