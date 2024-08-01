using ConsiliumTempus.Application.Project.Events;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Project.Events;

namespace ConsiliumTempus.Application.UnitTests.Project.Events;

public class AllowedMemberRemovedFromProjectHandlerTest
{
    #region Setup

    private readonly AllowedMemberRemovedFromProjectHandler _uut;

    public AllowedMemberRemovedFromProjectHandlerTest()
    {
        _uut = new AllowedMemberRemovedFromProjectHandler();
    }

    #endregion

    [Fact]
    public async Task HandleAllowedMemberRemovedFromProject_WhenSucceeds_ShouldRemoveProjectFromUsersFavorites()
    {
        // Arrange
        var project = ProjectFactory.CreateWithAllowedMembers();
        var user = project.AllowedMembers[1];
        project.UpdateFavorites(true, user);
        var domainEvent = new AllowedMemberRemovedFromProject(project, user);

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        project.Favorites.Should().NotContain(u => u != user);
    }
}