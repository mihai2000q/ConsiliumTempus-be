using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Project.Queries.GetCollectionForUser;

public class GetCollectionProjectForUserQueryHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProjectRepository _projectRepository;
    private readonly GetCollectionProjectForUserQueryHandler _uut;

    public GetCollectionProjectForUserQueryHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _projectRepository = Substitute.For<IProjectRepository>();
        _uut = new GetCollectionProjectForUserQueryHandler(_currentUserProvider, _projectRepository);
    }

    #endregion

    [Fact]
    public async Task WhenGetCollectionProjectForUserFails_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetCollectionProjectForUserQuery();

        _currentUserProvider
            .GetCurrentUser()
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();
        _projectRepository.DidNotReceive();
        
        outcome.ValidateError(Errors.User.NotFound);
    }
    
    [Fact]
    public async Task WhenGetCollectionProjectForUserSucceeds_ShouldReturnProjects()
    {
        // Arrange
        var query = ProjectQueryFactory.CreateGetCollectionProjectForUserQuery();

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUser()
            .Returns(user);

        var projects = ProjectFactory.CreateList();
        _projectRepository
            .GetListByUser(user.Id)
            .Returns(projects);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();
        await _projectRepository
            .Received(1)
            .GetListByUser(Arg.Any<UserId>());

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().BeEquivalentTo(new GetCollectionProjectForUserResult(projects));
    }
}