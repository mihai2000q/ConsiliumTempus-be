using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Commands.AddAllowedMember;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.AddAllowedMember;

public class AddAllowedMemberToProjectCommandHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly AddAllowedMemberToProjectCommandHandler _uut;

    public AddAllowedMemberToProjectCommandHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new AddAllowedMemberToProjectCommandHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleAddAllowedMemberToProjectCommand_WhenIsSuccessful_ShouldAddAllowedMemberToProject()
    {
        // Arrange
        var collaborator = UserFactory.Create();
        var workspace = WorkspaceFactory.Create();
        workspace.AddUserMembership(MembershipFactory.Create(collaborator));
        var project = ProjectFactory.Create(workspace: workspace, isPrivate: true);
        _projectRepository
            .GetWithCollaborators(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateAddAllowedMemberToProjectCommand(
            id: project.Id.Value,
            allowedMemberId: collaborator.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new AddAllowedMemberToProjectResult());

        Utils.Project.AssertFromAddAllowedMemberCommand(project, command, collaborator);
    }

    [Fact]
    public async Task HandleAddAllowedMemberToProjectCommand_WhenIsAlreadyAllowedMember_ShouldReturnAlreadyAllowedMemberError()
    {
        // Arrange
        var allowedMember = UserFactory.Create();
        var workspace = WorkspaceFactory.Create();
        workspace.AddUserMembership(MembershipFactory.Create(allowedMember));
        var project = ProjectFactory.Create(workspace: workspace, isPrivate: true);
        project.AddAllowedMember(allowedMember);
        _projectRepository
            .GetWithCollaborators(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateAddAllowedMemberToProjectCommand(
            id: project.Id.Value,
            allowedMemberId: allowedMember.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.ValidateError(Errors.Project.AlreadyAllowedMember);
    }

    [Fact]
    public async Task HandleAddAllowedMemberToProjectCommand_WhenCollaboratorIsNull_ShouldReturnCollaboratorNotFoundError()
    {
        // Arrange
        var project = ProjectFactory.Create(isPrivate: true);
        _projectRepository
            .GetWithCollaborators(Arg.Any<ProjectId>())
            .Returns(project);

        var command = ProjectCommandFactory.CreateAddAllowedMemberToProjectCommand(id: project.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.ValidateError(Errors.Workspace.CollaboratorNotFound);
    }
    
    [Fact]
    public async Task HandleAddAllowedMemberToProjectCommand_WhenProjectIsNotPrivate_ShouldReturnNotPrivateError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateAddAllowedMemberToProjectCommand();

        var project = ProjectFactory.Create(isPrivate: false);
        _projectRepository
            .GetWithCollaborators(Arg.Any<ProjectId>())
            .Returns(project);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.ValidateError(Errors.Project.NotPrivate);
    }

    [Fact]
    public async Task HandleAddAllowedMemberToProjectCommand_WhenProjectIsNull_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = ProjectCommandFactory.CreateAddAllowedMemberToProjectCommand();

        _projectRepository
            .GetWithCollaborators(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .GetWithCollaborators(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.ValidateError(Errors.Project.NotFound);
    }
}