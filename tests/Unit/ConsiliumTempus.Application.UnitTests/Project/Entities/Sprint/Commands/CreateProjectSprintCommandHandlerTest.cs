using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Sprint.Commands;

public class CreateProjectSprintCommandHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateProjectSprintCommandHandler _uut;
    
    public CreateProjectSprintCommandHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _uut = new CreateProjectSprintCommandHandler(_projectRepository, _projectSprintRepository, _unitOfWork);
    }

    #endregion

    [Fact]
    public async Task WhenCreateProjectSprintIsSuccessful_ShouldAddAndReturnSuccessResult()
    {
        // Arrange
        var command = new CreateProjectSprintCommand(
            Guid.NewGuid(), 
            "New Project Sprint",
            null,
            null);

        var project = Mock.Mock.Project.CreateMock();
        _projectRepository
            .GetWithWorkspace(Arg.Any<ProjectId>())
            .Returns(project);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectId>(id => id.Value == command.ProjectId));
        await _projectSprintRepository
            .Received(1)
            .Add(Arg.Is<ProjectSprint>(ps => 
                Utils.ProjectSprint.AssertFromCreateCommand(ps, command, project)));
        await _unitOfWork
            .Received(1)
            .SaveChangesAsync();
        
        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new CreateProjectSprintResult());
    }
    
    [Fact]
    public async Task WhenCreateProjectSprintFails_ShouldReturnProjectNotFoundError()
    {
        // Arrange
        var command = new CreateProjectSprintCommand(
            Guid.NewGuid(), 
            "New Project Sprint",
            null,
            null);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetWithWorkspace(Arg.Is<ProjectId>(id => id.Value == command.ProjectId));
        _projectSprintRepository.DidNotReceive();
        _unitOfWork.DidNotReceive();
        
        outcome.ValidateError(Errors.Project.NotFound);
    }
}