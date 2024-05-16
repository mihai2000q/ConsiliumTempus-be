using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectSprint.Queries.Get;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Sprint.Queries.Get;

public class GetProjectSprintQueryHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly GetProjectSprintQueryHandler _uut;

    public GetProjectSprintQueryHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new GetProjectSprintQueryHandler(_projectSprintRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetProjectSprintQuery_WhenIsSuccessful_ShouldReturnSprint()
    {
        // Arrange
        var query = ProjectSprintQueryFactory.CreateGetProjectSprintQuery();

        var projectSprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .Get(Arg.Any<ProjectSprintId>())
            .Returns(projectSprint);
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .Get(Arg.Is<ProjectSprintId>(id => query.Id == id.Value));

        outcome.IsError.Should().BeFalse();
        Utils.ProjectSprint.AssertProjectSprint(outcome.Value, projectSprint);
    }

    [Fact]
    public async Task HandleGetProjectSprintQuery_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = ProjectSprintQueryFactory.CreateGetProjectSprintQuery();

        _projectSprintRepository
            .Get(Arg.Any<ProjectSprintId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .Get(Arg.Is<ProjectSprintId>(id => query.Id == id.Value));

        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }
}