using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Project.Queries.GetAllowedMembers;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.GetAllowedMembers;

public class GetAllowedMembersFromProjectQueryHandlerTest
{
    #region Setup

    private readonly IProjectRepository _projectRepository;
    private readonly GetAllowedMembersFromProjectQueryHandler _uut;

    public GetAllowedMembersFromProjectQueryHandlerTest()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new GetAllowedMembersFromProjectQueryHandler(_projectRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetAllowedMembersFromProjectQuery_WhenIsSuccessful_ShouldReturnAllowedMembers()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetAllowedMembersFromProjectQuery();

        var project = ProjectFactory.CreateWithAllowedMembers();
        _projectRepository
            .GetWithAllowedMembers(Arg.Any<ProjectId>())
            .Returns(project);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetWithAllowedMembers(Arg.Is<ProjectId>(id => query.Id == id.Value));

        outcome.IsError.Should().BeFalse();
        outcome.Value.AllowedMembers.Should().BeEquivalentTo(project.AllowedMembers);
    }

    [Fact]
    public async Task HandleGetAllowedMembersFromProjectQuery_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetAllowedMembersFromProjectQuery();

        _projectRepository
            .GetWithAllowedMembers(Arg.Any<ProjectId>())
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _projectRepository
            .Received(1)
            .GetWithAllowedMembers(Arg.Is<ProjectId>(id => query.Id == id.Value));

        outcome.ValidateError(Errors.Project.NotFound);
    }
}