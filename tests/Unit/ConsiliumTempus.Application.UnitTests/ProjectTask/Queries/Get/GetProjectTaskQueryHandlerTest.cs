using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.ProjectTask.Queries.Get;

public class GetProjectTaskQueryHandlerTest
{
    #region Setup

    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly GetProjectTaskQueryHandler _uut;

    public GetProjectTaskQueryHandlerTest()
    {
        _projectTaskRepository = Substitute.For<IProjectTaskRepository>();
        _uut = new GetProjectTaskQueryHandler(_projectTaskRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetProjectTaskQuery_WhenIsSuccessful_ShouldReturnProjectTask()
    {
        // Arrange
        var query = ProjectTaskQueryFactory.CreateGetProjectTaskQuery();

        var task = ProjectTaskFactory.Create();
        _projectTaskRepository
            .Get(Arg.Any<ProjectTaskId>())
            .Returns(task);
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .Get(Arg.Is<ProjectTaskId>(id => query.Id == id.Value));

        outcome.IsError.Should().BeFalse();
        Utils.ProjectTask.AssertProjectTask(outcome.Value, task);
    }

    [Fact]
    public async Task HandleGetProjectTaskQuery_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = ProjectTaskQueryFactory.CreateGetProjectTaskQuery();

        _projectTaskRepository
            .Get(Arg.Any<ProjectTaskId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectTaskRepository
            .Received(1)
            .Get(Arg.Is<ProjectTaskId>(id => query.Id == id.Value));

        outcome.ValidateError(Errors.ProjectTask.NotFound);
    }
}