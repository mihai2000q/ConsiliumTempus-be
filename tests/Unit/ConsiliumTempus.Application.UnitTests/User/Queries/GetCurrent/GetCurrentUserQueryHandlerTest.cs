using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Queries.GetCurrent;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.User.Queries.GetCurrent;

public class GetCurrentUserQueryHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly GetCurrentUserQueryHandler _uut;

    public GetCurrentUserQueryHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _uut = new GetCurrentUserQueryHandler(_currentUserProvider);
    }

    #endregion

    [Fact]
    public async Task HandleGetCurrentUserQuery_WhenIsSuccessful_ShouldReturnCurrentUser()
    {
        // Arrange
        var query = new GetCurrentUserQuery();

        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUser()
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(query, CancellationToken.None);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();

        outcome.IsError.Should().BeFalse();
        Utils.User.AssertUser(outcome.Value, user);
    }

    [Fact]
    public async Task HandleGetCurrentUserQuery_WhenUserIsNull_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var query = new GetCurrentUserQuery();

        _currentUserProvider
            .GetCurrentUser()
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(query, CancellationToken.None);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();

        outcome.ValidateError(Errors.User.NotFound);
    }
}