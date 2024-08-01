using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.Project.Commands.LeavePrivate;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.LeavePrivate;

public class LeavePrivateProjectCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProjectRepository _projectRepository;
    private readonly LeavePrivateProjectCommandHandler _uut;

    public LeavePrivateProjectCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new LeavePrivateProjectCommandHandler(_currentUserProvider, _projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleLeavePrivateProjectCommand_WhenIsSuccessful_ShouldLeavePrivateProject()
    {
        // Arrange
        var project = ProjectFactory.CreateWithAllowedMembers(isPrivate: true);
        _projectRepository
            .GetWithAllowedMembers(Arg.Any<ProjectId>())
            .Returns(project);

        var allowedMember = project.AllowedMembers[1];
        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(allowedMember);

        var command = ProjectCommandFactory.CreateLeavePrivateProjectCommand(project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .GetWithAllowedMembers(Arg.Is<ProjectId>(pId => pId.Value == command.Id));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new LeavePrivateProjectResult());

        Utils.Project.AssertFromLeavePrivateCommand(project, command, allowedMember);
    }

    [Fact]
    public async Task
        HandleLeavePrivateProjectCommand_WhenCommandHasCurrentUserAsAllowedMember_ShouldReturnLeaveOwnedError()
    {
        // Arrange
        var project = ProjectFactory.CreateWithAllowedMembers(isPrivate: true);
        _projectRepository
            .GetWithAllowedMembers(Arg.Any<ProjectId>())
            .Returns(project);

        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(project.Owner);

        var command = ProjectCommandFactory.CreateLeavePrivateProjectCommand(project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .GetWithAllowedMembers(Arg.Is<ProjectId>(pId => pId.Value == command.Id));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        outcome.ValidateError(Errors.Project.LeaveOwned);
    }

    [Fact]
    public async Task HandleLeavePrivateProjectCommand_WhenProjectIsNotPrivate_ShouldReturnNotPrivateError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateLeavePrivateProjectCommand();

        var project = ProjectFactory.Create(isPrivate: false);
        _projectRepository
            .GetWithAllowedMembers(Arg.Any<ProjectId>())
            .Returns(project);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .GetWithAllowedMembers(Arg.Is<ProjectId>(pId => pId.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.Project.NotPrivate);
    }

    [Fact]
    public async Task HandleLeavePrivateProjectCommand_WhenProjectIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateLeavePrivateProjectCommand();

        _projectRepository
            .GetWithAllowedMembers(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .GetWithAllowedMembers(Arg.Is<ProjectId>(pId => pId.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.Project.NotFound);
    }
}