using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Entities.Sprint.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.Project.Entities.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Sprint.Queries.GetCollection;

public class GetCollectionProjectSprintQueryHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly GetCollectionProjectSprintQueryHandler _uut;

    public GetCollectionProjectSprintQueryHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _uut = new GetCollectionProjectSprintQueryHandler(_projectRepository, _projectSprintRepository);
    }
    
    #endregion

    [Fact]
    public async Task HandleGetCollectionProjectSprintQuery_WhenSuccessful_ShouldReturnProjectSprints()
    {
        // Arrange
        var query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery();

        var project = ProjectFactory.Create();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);

        var sprints = ProjectSprintFactory.CreateList();
        _projectSprintRepository
            .GetListByProject(Arg.Any<ProjectId>())
            .Returns(sprints);
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(pId => pId.Value == query.ProjectId));

        await _projectSprintRepository
            .Received(1)
            .GetListByProject(Arg.Is<ProjectId>(pId => pId == project.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Sprints.Should().BeEquivalentTo(sprints);
    }
    
    [Fact]
    public async Task HandleGetCollectionProjectSprintQuery_WhenProjectIsNull_ShouldReturnProjectNotFoundError()
    {
        // Arrange
        var query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery();

        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .ReturnsNull();
        
        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(pId => pId.Value == query.ProjectId));

        _projectSprintRepository.DidNotReceive();

        outcome.ValidateError(Errors.Project.NotFound);
    }
}