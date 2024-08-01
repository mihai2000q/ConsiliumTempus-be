using ConsiliumTempus.Application.Project.Events;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Project.Events;

namespace ConsiliumTempus.Application.UnitTests.Project.Events;

public class RemovedAllowedMemberFromProjectHandlerTest
{
    #region Setup

    private readonly RemovedAllowedMemberFromProjectHandler _uut;

    public RemovedAllowedMemberFromProjectHandlerTest()
    {
        _uut = new RemovedAllowedMemberFromProjectHandler();
    }

    #endregion

    [Fact]
    public async Task HandleRemovedAllowedMemberFromProject_WhenSucceeds_ShouldRemoveProjectFromUsersFavorites()
    {
        // Arrange
        var project = ProjectFactory.CreateWithAllowedMembers();
        var user = project.AllowedMembers[1];
        project.UpdateFavorites(true, user);
        var domainEvent = new RemovedAllowedMemberFromProject(project, user);

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        project.Favorites.Should().NotContain(u => u != user);
    }
}