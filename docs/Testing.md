# Consilium Tempus Backend Testing

- [Unit Testing](#unit-testing)
    - [Coding Convention](#coding-convention)
- [Integration Testing](#integration-testing)
    - [Coding Convention](#coding-convention-1)

The backend of the application is tested using the **xUnit** framework. The tests are divided into Unit and Integration Tests.

## Unit Testing

The Unit tests are designed to test single units of code in isolation. They are quick and easy to create, and help find and fix bugs early in the development cycle. They can be typically run using the IDE by right clicking any of them. 

Otherwise the following command can be used in the terminal:
```sh
dotnet test --filter "FullyQualifiedName~UnitTests"
```

### Coding Convention

There is exactly one test class for each important class, excluding POJOs (i.e., for each handler there will be a corresponding test class). <br> 
The name of the test **class** will be the class under testing followed by the suffix "Test". <br>
The name of the **variable** component under testing is gonna be called **uut**(Unit Under Testing).  
The name of the test **methods** will be _T1_T2_ where _T1_ is what we are actually testing and _T2_ is the expected outcome. <br>
Inside each class there will be a header **region** named _Setup_. <br>
Inside each method there will be **3 stages**:
- Arrange - to prepare what needs to be done before acting (i.e., intialize test variables)
- Act - run the method that is being tested and store the result, if needed
- Assert - make all the necessary assertions to see if the outcome is the one desired (up to the developer to choose what is relevant)


For example, if we are unit testing the Register Command Handler, see the code below:

```csharp
public class RegisterCommandHandlerTest
{
    #region Setup

    private readonly RegisterCommandHandler _uut = new();

    public RegisterCommandHandlerTest() 
    {
        _uut = new RegisterCommandHandler();
    }

    #endregion

    [Fact]
    public async Task WhenRegisterIsSuccessful_ShouldCreateUserAndReturnNewToken() 
    {
        // Arrange
        var command = RegisterCommand("Example@Email.com");

        // Act
        var outcome = await _uut.Handle(command);

        // Assert
        outcome.Token.Should().Be("token for the user Example@Email.com")
    }
} 
```

## Integration Testing

DO THE ENVIRONMENT BEFORE HAND

To run the Integration Tests, type the following in the terminal:

```sh
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

### Coding Convention

The name of the test **class** will be the class under testing followed by the suffix "IntegrationTest". <br>
The name of the **variable** component under testing is gonna be called **sut** for Integration Tests.  
The name of the test **methods** will be _T1_T2_ where _T1_ is what we are actually testing and _T2_ is the expected outcome. <br>
Inside each class there will be a header **region** named _Setup_. <br>
Inside each method there will be **3 stages**:
- Arrange - to prepare what needs to be done before acting (i.e., intialize test variables)
- Act - run the method that is being tested and store the result, if needed
- Assert - make all the necessary assertions to see if the outcome is the one desired (up to the developer to choose what is relevant)


For example, if we are unit testing the Register Command Handler, see the code below:

```csharp
public class RegisterCommandHandlerIntegrationTest
{
    #region Setup

    private readonly RegisterCommandHandler _sut = new();

    public RegisterCommandHandlerIntegrationTest() 
    {
        _uut = new RegisterCommandHandler();
    }

    #endregion

    [Fact]
    public async Task WhenRegisterIsSuccessfull_ShouldCreateUserAndReturnNewToken() {
        // Arrange
        var command = RegisterCommand("Example@Email.com");

        // Act
        var outcome = await _sut.Handle(command);

        // Assert
        outcome.Token.Should().Be("token for the user Example@Email.com")
    }
} 
```