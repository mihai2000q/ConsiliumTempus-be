using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.User.Queries;

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
        var query = new GetUserQuery(Guid.NewGuid());

        var user = Mock.Mock.User.CreateMock();
        _userRepository
            .Get(Arg.Any<UserId>())
            .Returns(user);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(id => Utils.User.AssertId(id, query.Id.ToString())));

        outcome.IsError.Should().BeFalse();
        Utils.User.AssertUser(outcome.Value, user);
    }

    [Fact]
    public async Task WhenGetUserQueryIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = new GetUserQuery(Guid.NewGuid());

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _userRepository
            .Received(1)
            .Get(Arg.Is<UserId>(id => Utils.User.AssertId(id, query.Id.ToString())));

        outcome.ValidateError(Errors.User.NotFound);
    }
}