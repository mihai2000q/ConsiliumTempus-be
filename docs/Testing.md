# Consilium Tempus Backend Testing

- [Unit Testing](#unit-testing)
    - [Coding Convention](#coding-convention)
- [Integration Testing](#integration-testing)
    - [Coding Convention](#coding-convention-1)
    - [Api Integration](#api-integration)

The backend of the application is tested using the **xUnit** framework. The tests are divided into Unit and Integration Tests.

## Unit Testing

The Unit tests are designed to test single units of code in isolation. They are quick and easy to create, and help find and fix bugs early in the development cycle. They can be typically run using the IDE by right clicking any of them. 

Otherwise, to run all the unit tests the following command can be used in the terminal:
```sh
dotnet test --filter "FullyQualifiedName~UnitTests"
```

### Coding Convention

There is exactly one test class for each important class, excluding POJOs (i.e., for each handler there will be a corresponding test class). <br> 
The name of the test **class** will be the class under testing followed by the suffix "Test". <br>
The name of the **variable** component under testing is gonna be called **uut** (Unit Under Testing).  
The name of the test **methods** will be _T1_T2_ where _T1_ is the scenario we are testing and _T2_ is the expected outcome. <br>
Inside each class there will be a header **region** named _Setup_. <br>
Inside each method there will be **3 stages**:
- Arrange - to prepare what needs to be done before acting (i.e., intialize test variables)
- Act - run the scenario that is being tested and store the results, if needed
- Assert - make all the necessary assertions to see if the outcome is the one desired (up to the developer to choose what is relevant)

For example, if we are unit testing the Register Command Handler, take a look on the code below:

```csharp
public class RegisterCommandHandlerTest
{
    #region Setup

    private readonly RegisterCommandHandler _uut;

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

Integration testing is a type of software testing where components of the software are gradually integrated and then tested as a unified group. Usually these components are already working well individually (Unit Tests), but they may break when integrated with other components.

In order for an integration test to run accordingly, it needs a backing database, however, it should not interfere with the development environment. For those reasons, the tests have been designed so that they open a new Docker container on each run. Additionally, all tests will share the same database, however, it will reset the state between each run.

Inside the Api Layer a configuration file is proposed for this Integration Testing environment, `appsettings.Testing.json`.

To run the Integration Tests, type the following in the terminal:

```sh
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

### Coding Convention

The name of the test **class** will be the class under testing followed by the suffix "Test" (the project already mentions that it is an integration Test). <br>
The name of the **variable** component under testing is gonna be called **sut** (System Under Testing).  
The name of the test **methods** will be _T1_T2_ where _T1_ is the scenario we are testing and _T2_ is the expected outcome. <br>
Inside each class there will be a header **region** named _Setup_. <br>
Inside each method there will be **3 stages**:
- Arrange - to prepare what needs to be done before acting (i.e., intialize test variables)
- Act - run the scenario that is being tested and store the results, if needed
- Assert - make all the necessary assertions to see if the outcome is the one desired (up to the developer to choose what is relevant)


For example, if we are testing the Register Command Handler, take a look on the code below:

```csharp
public class RegisterCommandHandlerTest
{
    #region Setup

    private readonly RegisterCommandHandler _sut;

    public RegisterCommandHandlerTest() 
    {
        _sut = new RegisterCommandHandler();
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

### Api Integration

The Presentation Layer is exposed through REST, therefore, in order to test it thoroughly an HTTP Client is needed.

Inside the project, the classes **Web Application Factory** and the **Base Integration Test** provide a client and minimal functionality to write Api Integration Tests.

Unlike the other layers, the Api replaces the *SUT* variable component with the **Client**, therefore it does not need the *Setup* region either.

#### Web Application Factory

The **Web Application Factory** class creates a Docker Container with the following credentials, by default:
- **password** - StrongPassword123 (can be changed)
- **username** - sa
- **database name** - master
 
Inside the constructor it removes all "real" instances of the database context and injects the above mentioned container. It also implements the Async Lifetime interface from the **xUnit Framework** which provides 2 methods for initialization and dispose. When initializing, it applies the following:
- apply the migrations to the database
- create a singleton http client
- finally, create a database connection in order to start the **Respawner** that will reset the state of the container after each running method

#### Base Integration Test

This class also implements the Async Lifetime so that it can reset the Database on dispose (which happens after each method run). This class also initializes a singleton http client from the factory, and the Jwt Settings from the Testing configuration. The database is capable of reinitializing state, therefore, inside the initialize task, it will add datasets from an sql file. The sub class will be able to mention the filename, if it wants a certain state of the database (only one dataset per test class). To add more datasets, go to the `MockData` package inside the project (do not add multi-line comments). 

This class is intended to be extended from each test so that the **Client** and the **Docker Container** are initialized. 
