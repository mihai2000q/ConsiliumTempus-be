using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.GetOverview;

public class GetOverviewProjectQueryHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly GetOverviewProjectQueryHandler _uut;

    public GetOverviewProjectQueryHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new GetOverviewProjectQueryHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetOverviewProjectQuery_WhenIsSuccessful_ShouldReturnProject()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetOverviewProjectQuery();

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
        Utils.Project.AssertProjectOverview(outcome.Value, project);
    }

    [Fact]
    public async Task HandleGetOverviewProjectQuery_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetOverviewProjectQuery();

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