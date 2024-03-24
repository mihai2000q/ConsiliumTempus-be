using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.User.Queries.GetId;
using ConsiliumTempus.Common.UnitTests.User;

namespace ConsiliumTempus.Application.UnitTests.User.Queries.GetId;

public class GetUserIdQueryHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly GetUserIdQueryHandler _uut;
    
    public GetUserIdQueryHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _uut = new GetUserIdQueryHandler(_currentUserProvider);
    }

    #endregion

    [Fact]
    public async Task WhenGetUserIdQueryIsSuccessful_ShouldReturnUserId()
    {
        // Arrange
        var query = UserQueryFactory.CreateGetUserIdQuery();

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUser()
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();

        outcome.IsError.Should().BeFalse();
        outcome.Value.Should().Be(user.Id);
    }
}