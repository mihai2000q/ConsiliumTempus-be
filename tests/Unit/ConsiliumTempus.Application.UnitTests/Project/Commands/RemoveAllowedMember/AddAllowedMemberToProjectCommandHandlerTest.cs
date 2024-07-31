using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Commands.RemoveAllowedMember;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Commands.RemoveAllowedMember;

public class RemoveAllowedMemberFromProjectCommandHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly RemoveAllowedMemberFromProjectCommandHandler _uut;

    public RemoveAllowedMemberFromProjectCommandHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new RemoveAllowedMemberFromProjectCommandHandler(_projectRepository);
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

        var command = ProjectCommandFactory.CreateRemoveAllowedMemberFromProjectCommand(
            project.Id.Value,
            allowedMember.Id.Value);

        // Act
        var outcome = await _uut.Handle(command, default);

        // Arrange
        await _projectRepository
            .Received(1)
            .GetWithAllowedMembers(Arg.Is<ProjectId>(pId => pId.Value == command.Id));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(new RemoveAllowedMemberFromProjectResult());

        Utils.Project.AssertFromRemoveAllowedMemberCommand(project, command);
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

        outcome.ValidateError(Errors.Project.NotFound);
    }
}