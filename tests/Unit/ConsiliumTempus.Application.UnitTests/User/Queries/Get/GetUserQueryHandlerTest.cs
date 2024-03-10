using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.User.Queries.Get;

public class GetUserQueryHandlerTest
{
    #region Setup

    private readonly IUserRepository _userRepository;
    private readonly GetUserQueryHandler _uut;

    public GetUserQueryHandlerTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _uut = new GetUserQueryHandler(_userRepository);
    }

    #endregion

    [Fact]
    public async Task WhenGetUserQueryIsSuccessful_ShouldReturnUser()
    {
        // Arrange
        var query = UserQueryFactory.CreateGetUserQuery();

        var user = UserFactory.Create();
        _userRepository
            .Get(Arg.Any<UserId>())
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(id => id.Value == query.Id));

        outcome.IsError.Should().BeFalse();
        Utils.User.AssertUser(outcome.Value, user);
    }

    [Fact]
    public async Task WhenGetUserQueryIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = UserQueryFactory.CreateGetUserQuery();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(id => id.Value == query.Id));

        outcome.ValidateError(Errors.User.NotFound);
    }
}