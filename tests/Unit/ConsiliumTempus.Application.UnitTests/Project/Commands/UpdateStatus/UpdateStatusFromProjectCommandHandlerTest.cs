using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.Project.Commands.UpdateStatus;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.UpdateStatus;

public class UpdateStatusFromProjectCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProjectRepository _projectRepository;
    private readonly UpdateStatusFromProjectCommandHandler _uut;

    public UpdateStatusFromProjectCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new UpdateStatusFromProjectCommandHandler(_currentUserProvider, _projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleUpdateStatusFromProjectCommand_WhenIsSuccessful_ShouldUpdateStatusFromProject()
    {
        // Arrange
        var project = ProjectFactory.CreateWithStatuses();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);
        
        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(user);
        
        var command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommand(
            id: project.Id.Value,
            statusId: project.Statuses[0].Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(pId => pId.Value == command.Id));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new UpdateStatusFromProjectResult());

        Utils.Project.AssertFromUpdateStatusCommand(project, command, user);
    }
    
    [Fact]
    public async Task HandleUpdateStatusFromProjectCommand_WhenProjectStatusIsNull_ShouldReturnStatusNotFoundError()
    {
        // Arrange
        var project = ProjectFactory.CreateWithStatuses();
        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .Returns(project);
        
        var command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommand(id: project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.ValidateError(Errors.ProjectStatus.NotFound);
    }

    [Fact]
    public async Task HandleUpdateStatusFromProjectCommand_WhenProjectIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommand();

        _projectRepository
            .Get(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .Get(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.ValidateError(Errors.Project.NotFound);
    }
}