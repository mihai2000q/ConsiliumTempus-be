using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.User.Queries;

public class GetUserQueryHandlerTest
{
    #region Setup

    private readonly Mock<IUserRepository> _userRepository;
    private readonly GetUserQueryHandler _uut;

    public GetUserQueryHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _uut = new GetUserQueryHandler(_userRepository.Object);
    }

    #endregion

    [Fact]
    public async Task WhenGetUserQueryIsSuccessful_ShouldReturnUser()
    {
        // Arrange
        var query = new GetUserQuery(Guid.NewGuid());

        var user = Mock.Mock.User.CreateMock();
        _userRepository.Setup(u => u.Get(It.IsAny<UserId>(), default))
            .ReturnsAsync(user);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        _userRepository.Verify(u =>
                u.Get(It.Is<UserId>(id => Utils.User.AssertUserId(id, query.Id.ToString())), 
                    default),
                Times.Once());

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
        _userRepository.Verify(u =>
                u.Get(It.Is<UserId>(id => Utils.User.AssertUserId(id, query.Id.ToString())), 
                    default),
            Times.Once());

        outcome.ValidateError(Errors.User.NotFound);
    }
}