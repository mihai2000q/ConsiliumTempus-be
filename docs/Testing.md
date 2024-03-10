# Consilium Tempus Backend Testing

* [Unit Testing](#unit-testing)
  * [Coding Convention](#coding-convention)
* [Integration Testing](#integration-testing)
  * [Coding Convention](#coding-convention-1)
  * [Api Integration](#api-integration)
    * [Web App Factory](#web-app-factory)
    * [Base Integration Test](#base-integration-test)
    * [App Http Client](#app-http-client)
    * [Test Auth Handler](#test-auth-handler)
    * [Collections](#collections)

The backend of the application is tested using the **xUnit** framework. 
The tests are divided into Unit and Integration Tests.

## Unit Testing

The Unit tests are designed to test single units of code in isolation. 
They are quick and facile to create, and help find and fix bugs early in the development cycle. 
They can be typically run using the IDE by right-clicking any of them. 

Otherwise, to run all the unit tests, the following command can be used in the terminal:
```sh
dotnet test --filter "FullyQualifiedName~UnitTests"
```

### Coding Convention

There is exactly one test class for each important class, excluding POJOs
(i.e., for each handler there will be a corresponding test class). 
<br> 
The name of the test **class** will be the class under testing followed by the suffix "Test." 
<br>
The name of the **variable** component under testing is going to be called **uut** (Unit Under Testing).  
The name of the test **methods** can be _T1_T2_, where _T1_ is the scenario we are testing,
and _T2_ is the expected outcome. 
<br>
_T1_T2_T3_ is also accepted, where *T1* is the method/component under test, 
*T2* the scenario and *T3* the expected outcome.
<br>
Inside each class there will be a header **region** named _Setup_. 
<br>
Inside each method there will be **3 stages**:
- Arrange—to prepare what needs to be done before acting (i.e., initialize test variables)
- Act—run the scenario that is being tested and store the results, if needed
- Assert—make all the necessary assertions to see if the outcome is the one desired
(up to the developer to choose what is relevant)

For example, if we are unit testing the Register Command Handler, take a look at the code below:

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

Integration testing is a type of software testing where components of the software are gradually integrated 
and then tested as a unified group. 
Usually these components are already working well individually (Unit Tests), 
but they may break when integrated with other components.

In order for an integration test to run accordingly, it needs a backing database, however, 
it should not interfere with the development environment. 
For those reasons, the tests have been designed so that they open a new Docker container on each run. 
Additionally, all tests will share the same database, however, it will reset the state between each run.

Inside the Api Layer a configuration file is proposed for this Integration Testing environment, `appsettings.Testing.json`.

To run the Integration Tests, type the following in the terminal:

```sh
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

### Coding Convention

The name of the test **class** will be the class under testing followed by the suffix "Test"
(the project already mentions that it is an integration Test). 
<br>
The name of the **variable** component under testing is going to be called **sut** (System Under Testing).  
The name of the test **methods** can be _T1_T2_, where _T1_ is the scenario we are testing,
and _T2_ is the expected outcome. 
<br>
_T1_T2_T3_ is also accepted, where *T1* is the method/component under test, 
*T2* the scenario and *T3* the expected outcome.
<br>
Inside each class there will be a header **region** named _Setup_. 
<br>
Inside each method there will be **3 stages**:
- Arrange—to prepare what needs to be done before acting (i.e., initialize test variables)
- Act—run the scenario that is being tested and store the results, if needed
- Assert—make all the necessary assertions to see if the outcome is the one desired
(up to the developer to choose what is relevant)


For example, if we are testing the Register Command Handler, take a look at the code below:

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
    public async Task HandleRegisterCommand_WhenValid_ShouldCreateUserAndReturnNewToken() {
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

The Presentation Layer is exposed through REST, therefore; to test it thoroughly, an HTTP Client is needed.

Inside the project, the **Core** directory contains the classes that are primordial for Api Integration Testing.
<br>
The core components are:
- Web App Factory
- Base Integration Test
- App Http Client
- Test Auth Handler

Together, they provide all the functionalities to write Api Integration Tests.

Unlike the other layers, the Api replaces the *SUT* variable component with the **Client**; 
therefore, it does not need the *Setup* region either.

#### Web App Factory

The **Web App Factory** class creates a Docker Container per *Collection* with the following credentials, by default:
- **password** = StrongPassword123 (can be changed inside the Constants class)
- **username** = sa
- **database name** = master
 
Inside the constructor it removes all "real"
instances of the database context and injects the above-mentioned container.
On construction, the Testing Environment is set to take in the parameters in the `appsettings.Testing.json`, 
and the **Test Auth Handler** is invoked to add authentication to the tests.
It also implements the Async Lifetime interface from the **xUnit Framework**
which provides two methods for initialization and dispose.
When initializing, it applies the following:
- apply the migrations to the database
- create a database connection to start the **Respawner** 
that will reset the state of the container after each running method

#### Base Integration Test

This class also implements the Async Lifetime so that it can reset the Database on disposing
(which happens after each method runs).
This class also instantiates the **App Http Client** and the Jwt Settings from the Testing configuration 
(`appsettings.Testing.json`).
The database is capable of reinitializing state, 
therefore, inside the initialize task method, 
it will add datasets from multiple sql files from within the Test Data directory.
The subclass will be able to mention the directory name
if it wants a certain state of the database (generally only one dataset per test collection).
To add more datasets, go to the `TestData` package inside the project 
(do not add multi-line comments) and add a directory with as many sql files you need.
Each sql file represents a table inside the database. 
Also, make sure they are in the right order, so that the constraints apply.

By default, inside the database there will be a dataset of Users 
(can be deactivated per Test on the constructor parameters).
Those Users can be modified inside the sole table inside the `TestData` directory (`User.sql`).

This class is intended to be extended for each test so that the **Client** and the **Docker Container** are initialized.

#### App Http Client

The **App Http Client** represents a wrapper class for the *Http Client* provided by dotnet core. 
Besides wrapping, it also provides a way to create custom tokens based on email.
To use a custom Token for each test method, make use of the method `UseCustomToken` and pass the email of the user.

#### Test Auth Handler

This class is used by the Web App Factory to let the Client bypass any Authentication Layer that requires it.
In other words, some endpoints will require some type of authorization and others might just accept anonymous requests.
However, those that require authorization will, typically, require a Token as well 
(which is already provided and discussed in the [Base Integration Test](#base-integration-test) class).

The Handler also makes use of the **Token Provider** interface, 
that is injecting a singleton service which provides a custom token 
(the method: *Use Custom Token* from the App Http Client class makes use of this provider).

#### Collections

Each package of the integration tests is generally going to share the initial database state
and then use **Respawner** to reinitialize the state. 
More precise, the testing strategy is one container per collection. 
To create a collection, go to `TestCollection` and add one that extends the `ICollectionFixture<WebAppFactory>`.

Generally, for an integration test to work,
it will need to be part of a *collection* and extend the *base integration test* class