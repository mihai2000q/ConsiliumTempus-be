using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project.Entities.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Project.Entities.Sprint.Commands.Delete;

public class DeleteProjectSprintCommandHandlerTest
{
    #region Setup

    private readonly IProjectSprintRepository _projectSprintRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteProjectSprintCommandHandler _uut;
    
    public DeleteProjectSprintCommandHandlerTest()
    {
        _projectSprintRepository = Substitute.For<IProjectSprintRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _uut = new DeleteProjectSprintCommandHandler(_projectSprintRepository, _unitOfWork);
    }

    #endregion

    [Fact]
    public async Task WhenDeleteProjectSprintIsSuccessful_ShouldRemoveAndReturnSuccessfulResult()
    {
        // Arrange
        var command = new DeleteProjectSprintCommand(Guid.NewGuid());

        var projectSprint = ProjectSprintFactory.Create();
        _projectSprintRepository
            .GetWithProjectAndWorkspace(Arg.Any<ProjectSprintId>())
            .Returns(projectSprint);
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithProjectAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _projectSprintRepository
            .Received(1)
            .Remove(Arg.Is<ProjectSprint>(ps => ps == projectSprint));
        await _unitOfWork
            .Received(1)
            .SaveChangesAsync();
        
        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new DeleteProjectSprintResult());
    }
    
    [Fact]
    public async Task WhenDeleteProjectSprintFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new DeleteProjectSprintCommand(Guid.NewGuid());
        
        // Act
        var outcome = await _uut.Handle(command, default);

        // Assert
        await _projectSprintRepository
            .Received(1)
            .GetWithProjectAndWorkspace(Arg.Is<ProjectSprintId>(id => id.Value == command.Id));
        _projectSprintRepository
            .DidNotReceive()
            .Remove(Arg.Any<ProjectSprint>());
        _unitOfWork.DidNotReceive();
        
        outcome.IsError.Should().BeTrue();
        outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }
}