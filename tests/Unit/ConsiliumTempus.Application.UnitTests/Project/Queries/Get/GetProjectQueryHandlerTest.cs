using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.Get;

public class GetProjectQueryHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly GetProjectQueryHandler _uut;

    public GetProjectQueryHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new GetProjectQueryHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetProjectQuery_WhenIsSuccessful_ShouldReturnProject()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetProjectQuery();

        var project = ProjectFactory.CreateWithSprints();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => query.Id == id.Value));

        outcome.IsError.Should().BeFalse();
        Utils.Project.AssertProject(outcome.Value, project);
    }

    [Fact]
    public async Task HandleGetProjectQuery_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetProjectQuery();

        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => query.Id == id.Value));

        outcome.ValidateError(Errors.Project.NotFound);
    }
}