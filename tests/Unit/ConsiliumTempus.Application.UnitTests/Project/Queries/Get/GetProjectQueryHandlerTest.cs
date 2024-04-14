using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;

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
    public async Task HandleGetWorkspaceQuery_WhenIsSuccessful_ShouldReturnWorkspace()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetProjectQuery();

        var project = ProjectFactory.Create();
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
    public async Task HandleGetWorkspaceQuery_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetProjectQuery();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(id => query.Id == id.Value));

        outcome.ValidateError(Errors.Project.NotFound);
    }
}