using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Authentication.Queries.Login;
using ConsiliumTempus.Domain.Common.Errors;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class AuthenticationControllerTest
{
    #region Setup

    private readonly Mock<ISender> _mediator;
    private readonly AuthenticationController _uut;

    public AuthenticationControllerTest()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(AuthenticationMappingConfig).Assembly);
        IMapper mapper = new Mapper(config);
        _mediator = new Mock<ISender>();
        _uut = new AuthenticationController(mapper, _mediator.Object);

        // Resolve Http Context
        Mock<HttpContext> httpContext = new();
        _uut.ControllerContext.HttpContext = httpContext.Object;
        httpContext.SetupGet(h => h.Items)
            .Returns(new Dictionary<object, object?>());
    }

    #endregion

    [Fact]
    public async Task WhenRegisterIsSuccessful_ShouldReturnRegisterResponse()
    {
        // Arrange
        var request = new RegisterRequest(
            "FirstName",
            "LastName",
            "Example@Email.com",
            "Password123");

        var result = new RegisterResult("This is the token for the registration");

        _mediator.Setup(m => m.Send(It.IsAny<RegisterCommand>(), default))
            .ReturnsAsync(result);

        // Act
        var outcome = await _uut.Register(request);

        // Assert
        _mediator.Verify(m => m.Send(
                It.Is<RegisterCommand>(command => AssertRegisterCommand(command, request)), 
                default),
            Times.Once());

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<RegisterResponse>();

        var response = ((OkObjectResult)outcome).Value as RegisterResponse;
        response?.Token.Should().Be(result.Token);
    }
    
    [Fact]
    public async Task WhenRegisterFails_ShouldReturnDuplicateEmailError()
    {
        // Arrange
        var request = new RegisterRequest(
            "FirstName",
            "LastName",
            "Example@Email.com",
            "Password123");
        
        _mediator.Setup(m => m.Send(It.IsAny<RegisterCommand>(), default))
            .ReturnsAsync(Errors.User.DuplicateEmail);

        // Act
        var outcome = await _uut.Register(request);

        // Assert
        _mediator.Verify(m => m.Send(
                It.Is<RegisterCommand>(command => AssertRegisterCommand(command, request)), 
                default),
            Times.Once());

        outcome.Should().BeOfType<ObjectResult>();
        ((ObjectResult)outcome).Value.Should().BeOfType<ProblemDetails>();
        
        var error = ((ObjectResult)outcome).Value as ProblemDetails;
        error?.Status.Should().Be(StatusCodes.Status409Conflict);
        error?.Title.Should().Be("Email is already in use");
    }

    [Fact]
    public async Task WhenLoginIsSuccessful_ShouldReturnLoginResponse()
    {
        // Arrange
        var request = new LoginRequest(
            "Some@Example.com",
            "Password123");

        var result = new LoginResult("This is the token");
        _mediator.Setup(m => m.Send(It.IsAny<LoginQuery>(), default))
            .ReturnsAsync(result);
        
        // Act
        var outcome = await _uut.Login(request);

        // Assert
        _mediator.Verify(m => m.Send(
                It.Is<LoginQuery>(query => AssertLoginQuery(query, request)), 
                default),
            Times.Once());

        outcome.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)outcome).Value.Should().BeOfType<LoginResponse>();
        
        var response = ((OkObjectResult)outcome).Value as LoginResponse;
        response?.Token.Should().Be(result.Token);
    }
    
    [Fact]
    public async Task WhenLoginFails_ShouldReturnLoginResponse()
    {
        // Arrange
        var request = new LoginRequest(
            "Some@Example.com",
            "Password123");

        _mediator.Setup(m => m.Send(It.IsAny<LoginQuery>(), default))
            .ReturnsAsync(Errors.Authentication.InvalidCredentials);
        
        // Act
        var outcome = await _uut.Login(request);

        // Assert
        _mediator.Verify(m => m.Send(
                It.Is<LoginQuery>(query => AssertLoginQuery(query, request)), 
                default),
            Times.Once());

        outcome.Should().BeOfType<ObjectResult>();
        ((ObjectResult)outcome).Value.Should().BeOfType<ProblemDetails>();
        
        var error = ((ObjectResult)outcome).Value as ProblemDetails;
        error?.Status.Should().Be(StatusCodes.Status401Unauthorized);
        error?.Title.Should().Be("Invalid Credentials");
    }

    private static bool AssertRegisterCommand(RegisterCommand command, RegisterRequest request)
    {
        command.FirstName.Should().Be(request.FirstName);
        command.LastName.Should().Be(request.LastName);
        command.Email.Should().Be(request.Email);
        command.Password.Should().Be(request.Password);
        return true;
    }
    
    private static bool AssertLoginQuery(LoginQuery query, LoginRequest request)
    {
        query.Email.Should().Be(request.Email);
        query.Password.Should().Be(request.Password);
        return true;
    }
}