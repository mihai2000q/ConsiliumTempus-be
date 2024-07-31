using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.Project.Commands.RemoveAllowedMember;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.RemoveAllowedMember;

public class RemoveAllowedMemberFromProjectCommandHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProjectRepository _projectRepository;
    private readonly RemoveAllowedMemberFromProjectCommandHandler _uut;

    public RemoveAllowedMemberFromProjectCommandHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new RemoveAllowedMemberFromProjectCommandHandler(_currentUserProvider, _projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleRemoveAllowedMemberFromProjectCommand_WhenIsSuccessful_ShouldRemoveAllowedMemberFromProject()
    {
        // Arrange
        var project = ProjectFactory.CreateWithAllowedMembers(isPrivate: true);
        var allowedMember = project.AllowedMembers[0];
        _projectRepository
            .GetWithAllowedMembers(Arg.Any<ProjectId>())
            .Returns(project);

        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(UserFactory.Create());

        var command = ProjectCommandFactory.CreateRemoveAllowedMemberFromProjectCommand(
            project.Id.Value,
            allowedMember.Id.Value);

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
        outcome.Value.Should().Be(new RemoveAllowedMemberFromProjectResult());

        Utils.Project.AssertFromRemoveAllowedMemberCommand(project, command);
    }

    [Fact]
    public async Task 
        HandleRemoveAllowedMemberFromProjectCommand_WhenCommandHasCurrentUserAsAllowedMember_ShouldReturnRemoveYourselfError()
    {
        // Arrange
        var project = ProjectFactory.CreateWithAllowedMembers(isPrivate: true);
        var allowedMember = project.AllowedMembers[0];
        _projectRepository
            .GetWithAllowedMembers(Arg.Any<ProjectId>())
            .Returns(project);

        _currentUserProvider
            .GetCurrentUserAfterPermissionCheck()
            .Returns(allowedMember);

        var command = ProjectCommandFactory.CreateRemoveAllowedMemberFromProjectCommand(
            project.Id.Value,
            allowedMember.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .GetWithAllowedMembers(Arg.Is<ProjectId>(pId => pId.Value == command.Id));
        await _currentUserProvider
            .Received(1)
            .GetCurrentUserAfterPermissionCheck();

        outcome.ValidateError(Errors.Project.RemoveYourself);
    }

    [Fact]
    public async Task HandleRemoveAllowedMemberFromProjectCommand_WhenAllowedMemberIsNull_ShouldReturnAllowedMemberNotFoundError()
    {
        // Arrange
        var project = ProjectFactory.Create(isPrivate: true);
        _projectRepository
            .GetWithAllowedMembers(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateRemoveAllowedMemberFromProjectCommand(id: project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .GetWithAllowedMembers(Arg.Is<ProjectId>(pId => pId.Value == command.Id));
        _currentUserProvider.DidNotReceive();

        outcome.ValidateError(Errors.Project.AllowedMemberNotFound);
    }
    
    [Fact]
    public async Task HandleRemoveAllowedMemberFromProjectCommand_WhenProjectIsNotPrivate_ShouldReturnNotPrivateError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateRemoveAllowedMemberFromProjectCommand();

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
    public async Task HandleRemoveAllowedMemberFromProjectCommand_WhenProjectIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateRemoveAllowedMemberFromProjectCommand();

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