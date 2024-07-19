# Consilium Tempus Feature Development Workflow

* [Creating a New Aggregate](#creating-a-new-aggregate)
  * [Domain](#domain)
    * [Value Objects](#value-objects)
    * [Aggregate](#aggregate)
    * [Errors](#errors)
    * [Properties Validation](#properties-validation)
  * [Infrastructure](#infrastructure)
    * [Configuration](#configuration)
* [Creating a New Command](#creating-a-new-command)
  * [Infrastructure](#infrastructure-1)
    * [Repository](#repository)
    * [Database Set](#database-set)
  * [Application](#application)
    * [Repository Interfaces](#repository-interfaces)
    * [Command](#command)
    * [Result](#result)
    * [Validator](#validator)
    * [Handler](#handler)
* [Creating a New Query](#creating-a-new-query)
  * [Application](#application-1)
    * [Query](#query)
    * [Validator](#validator-1)
    * [Handler](#handler-1)
* [Creating a New Endpoint](#creating-a-new-endpoint)
  * [Api](#api)
    * [Controller](#controller)
    * [Contracts](#contracts)
    * [Mapping](#mapping)
    * [Endpoint](#endpoint)

Implementing a new feature in this application follows the clean architecture principles,
to put it in simple words:

- the request comes through a **presentation** layer, in our case, a REST api,
- it is, optionally, run through a security check inside the infrastructure
- inside the endpoint,
  - it gets mapped to a query or a command
  - and sent through the mediator to get handled
- inside the **application** layer,
  - the command or query gets validated
  - handled
  - optionally, it requests data from the database through a repository
  - and returns a result or an error
- finally, it gets mapped again to a response and is sent to the user

## Creating a New Aggregate

If the aggregate exists already, then jump to [Creating a New Command](#creating-a-new-command).

### Domain

#### Value Objects

Inside the **Domain** layer, we will start by creating the **Aggregate** class,
under a package with the same name (Project).

First, we will create the strong typed id of the Project, under the package **Project/ValueObjects**:

```csharp
public sealed class ProjectId : AggregateRootId<Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectId()
    {
    }

    private ProjectId(Guid value)
    {
        Value = value;
    }

    public override Guid Value { get; protected set; }

    public static ProjectId CreateUnique()
    {
        return new ProjectId(Guid.NewGuid());
    }

    public static ProjectId Create(Guid value)
    {
        return new ProjectId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
```

Notice the following:

- the sealed class inherits from the **AggregateRootId** which requires the type of the Id
- again, the private no args constructor for EF Core
- private default constructor
- properties (only the value of the Id)
- two methods to instantiate the Id, one that creates a new unique one, and the other based on a Guid
  (used later by ef core and when querying from the database by id)
- finally, each value object class should override the method **GetEqualityComponents** and return its properties,
  in our case only the Value of the id

Another value object that the project aggregate will use is the **Name**,
but its implementation will be a look alike with the **ProjectId**.

#### Aggregate

Next, we will create the **Aggregate** class, inside the package **Project**:

```csharp
public sealed class ProjectAggregate : AggregateRoot<ProjectId, Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectAggregate()
    {
    }

    private ProjectAggregate(
        ProjectId id,
        Name name,
        UserAggregate user,
        DateTime createdDateTime) : base(id)
    {
        Name = name;
        User = user;
        CreatedDateTime = createdDateTime;
    }

    public Name Name { get; private set; } = default!;
    public UserAggregate User { get; private set; } = default!;
    public DateTime CreatedDateTime { get; init; }

    public static ProjectAggregate Create(
        Name name,
        UserAggregate user)
    {
        return new ProjectAggregate(
            ProjectId.CreateUnique(),
            name,
            user,
            DateTime.UtcNow);
    }
}
```

Notice the following:

- the sealed class inherits from our domain's **AggregateRoot**, which needs a strong-typed Id and the type of the Id.
- the private no args constructor is needed for **Entity Framework Core** to create them from the database using
  reflection
- the default constructor should be private, and it calls the base one with the Id
- the properties of the aggregate are just below
- and, to create a new aggregate, the developer should use the static **Create** method,
  which should take as parameters only the fields that can be chosen by the user upon creation.
  (i.e., the created date time is not a user choice, as it should always be, upon creation, the actual date and time)

#### Errors

Let's also create an error in case the user cannot be found,
inside the package **Common/Errors**, inside the file **Errors.User**:

```csharp
public static partial class Errors
{
    public static class User
    {
        public static Error NotFound => Error.NotFound(
            "User.NotFound",
            "User could not be found");
    }
}
```

#### Properties Validation

Similarly, we will add a limit to how long the name of a project can be,
inside the package **Common/Validation**, inside the file **PropertiesValidation.Project**:

```csharp
public static partial class PropertiesValidation
{
    public static class Project
    {
        public const short NameMaximumLength = 100;
    }
}
```

### Infrastructure

#### Configuration

Now, in order for Entity Framework to be able to create this class upon query, it should be configured.

Inside the **Infrastructure** layer, under the **Persistence/Configuration** package,
each aggregate has its own configuration, as following:

```csharp
public sealed class ProjectConfiguration : IEntityTypeConfiguration<ProjectAggregate>
{
    public void Configure(EntityTypeBuilder<ProjectAggregate> builder)
    {
        builder.ToTable(nameof(ProjectAggregate).TruncateAggregate());

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value));

        builder.OwnsOne(p => p.Name)
            .Property(n => n.Value)
            .HasColumnName(nameof(Name))
            .HasMaxLength(PropertiesValidation.Project.NameMaximumLength);

        builder.HasOne(p => p.User)
            .WithMany(u => u.Projects);
    }
}
```

Usually, after creating or updating a configuration, a migration should be created.
More information about migrations can be found [here](Database.md/#migrations).

## Creating a New Command

Let's take in consideration the implementation of a new endpoint that creates a project per user
(the actual endpoint is created [here](#creating-a-new-endpoint)).

### Infrastructure

Inside the **Infrastructure** layer, we will create the following:

- the repositories
- and the database sets, if needed

#### Repository

First, we have to create a repository where the data can be queried from the database.
Usually, there will be one repository per aggregate, and it will contain all the required methods.
Also, the repositories can be found/created under the **Persistence/Repository** package.

Let's proceed by creating the repository for the user and query it from the database.

```csharp
public sealed class UserRepository(ConsiliumTempusDbContext dbContext)
{
    public async Task<UserAggregate?> Get(UserId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.FindAsync([id], cancellationToken);
    }
}
```

The above-sealed class injects the database context, and is using it to query the user from the database.

```csharp
public sealed class ProjectRepository(ConsiliumTempusDbContext dbContext)
{
    public async Task Add(ProjectAggregate project, CancellationToken cancellationToken = default)
    {
        await dbContext.Projects.AddAsync(project, cancellationToken);
    }
}
```

Similarly, the project repository uses the database context to add a new project.

#### Database Set

If the database set is not defined already, under the **Persistence/Database** package,
inside the **ConsiliumTempusDbContext**, one can define a new database set, likewise:

```csharp
public sealed class ConsiliumTempusDbContext() {
    public DbSet<UserAggregate> Users { get; init; } = null!;
    public DbSet<ProjectAggregate> Projects { get; init; } = null!;
}
```

### Application

Inside the **Application** layer, we will create the following:

- interfaces for the repositories created in the infrastructure
- the command
- its result
- the command validator
- and the command handler

#### Repository Interfaces

If the repositories have just been created, get on with the following guidelines, otherwise, jump
to [Command](#command);

In order for the application to communicate with the database, it uses interfaces on the infrastructure layer.

Under the package **Common/Interfaces/Repository**, we can create two interfaces for the repositories:

```csharp
public interface IUserRepository
{
    Task<UserAggregate?> Get(UserId id, CancellationToken cancellationToken = default);
}
```

And:

```csharp
public interface IProjectRepository
{
    Task Add(ProjectAggregate project, CancellationToken cancellationToken = default);
}
```

Now we can go back on the Infrastructure Layer to declare them, so that we can use dependency injection.

First, we will implement them on the concrete repositories as the following:

```csharp
public sealed class UserRepository(ConsiliumTempusDbContext dbContext) : IUserRepository {
  ...
}
```

And:

```csharp
public sealed class ProjectRepository(ConsiliumTempusDbContext dbContext) : IProjectRepository {
  ...
}
```

Second, under the root folder, inside the **Dependency Injection** class, on the bottom,
there is already a method called **AddRepositories**.

There, we can add our repositories:

```csharp
public static class DependencyInjection
{
    ...

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
```

#### Command

Next, we will create the new **Command**, inside the **Project/Commands/Create** package:

```csharp
public sealed record CreateProjectCommand(
    Guid UserId,
    string Name)
    : IRequest<ErrorOr<CreateProjectResult>>;
```

For this example, we will keep it simple and create a project for a user with only its name.

Notice the following:

- it is a record instead of a class
- it is sealed
- the fields inside it are not strong-typed, and they should match both the name and type of the request
- it implements the **IRequest** interface from _MediatR_, which also mentions the return type
- the return type of the command has generic ErrorOr and the expected **Result**

#### Result

Let's create the result class:

```csharp
public sealed record CreateProjectResult(ProjectAggregate Project);
```

Notice that this is also a sealed record and it just returns the newly created project.

#### Validator

Usually, a command will need validation before it can be handled.
This validator will be assigned to the command on runtime via the behavior pipeline.

```csharp
public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty();
            
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(PropertiesValidation.Project.NameMaximumLength);
    }
}
```

The above validates that:

- the id of the user cannot be empty
- the name of the new project cannot be empty and it has a maximum length

Notice the following:

- the class is sealed
- it inherits from **Abstract Validator** and generically declares the **Command**
- inside the constructor you can define the validation rules via **Fluent Validator**

#### Handler

Now, to create the actual business logic, we will write a **Handler** for the command.

```csharp
public sealed class CreateProjectCommandHandler(
    IUserRepository userRepository,
    IProjectRepository projectRepository)
    : IRequestHandler<CreateProjectCommand, ErrorOr<CreateProjectResult>>
{
    public async Task<ErrorOr<CreateProjectResult>> Handle(CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.Get(
            UserId.Create(command.UserId),
            cancellationToken);
        if (user is null) return Errors.User.NotFound;

        var project = ProjectAggregate.Create(
            Name.Create(command.Name),
            user);
        await projectRepository.Add(project, cancellationToken);

        return new CreateProjectResult(project);
    }
}
```

To explain the above, it starts by injecting two repositories, user and project,
and it implements the interface **IRequestHandler** from _MediatR_,
which generically needs the command and its result (both already written in the [command](#command) class).

Then it implements the Handle method, where the actual logic happens.

The above class:

- looks for the user, returns an error in case one could not be found
- then, it creates a project
- it adds the project to the database
- and finally, it returns the successful message under the result class

## Creating a New Query

Let's take in consideration the implementation of a new endpoint that retrieves a user
(the actual endpoint won't be exemplified).

### Application

#### Query

We will start by creating the new **Query** inside the application layer,
inside the **User/Queries/Get** package:

```csharp
public sealed record GetUserQuery(Guid Id)
    : IRequest<ErrorOr<UserAggregate>>;
```

For this example, we will keep it simple and get a user by id.

Notice the following:

- it is a record instead of a class
- it is sealed
- the fields inside it are not strong-typed, and they should match both the name and type of the request
- it implements the **IRequest** interface from _MediatR_, which also mentions the return type
- the return type of the query has generic ErrorOr and the domain aggregate
  (it could also be encapsulated under a result record)

#### Validator

Usually, a query will need validation before it can be handled.
This validator will be assigned to the command on runtime via the behavior pipeline.

```csharp
public sealed class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty();
    }
}
```

The above validates that the id of the user cannot be empty

Notice the following:

- the class is sealed
- it inherits from **Abstract Validator** and generically declares the **Query**
- inside the constructor you can define the validation rules via **Fluent Validator**

#### Handler

Now, to create the actual business logic, we will write a **Handler** for the query.

```csharp
public sealed class GetUserQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserQuery, ErrorOr<UserAggregate>>
{
    public async Task<ErrorOr<UserAggregate>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.Get(UserId.Create(command.UserId), cancellationToken);
        return user is null ? Errors.User.NotFound : user;
    }
}
```

To explain the above, it starts by injecting the user repository (created [here](#repository)),
it implements the interface **IRequestHandler** from _MediatR_,
which generically requires the query and its result (both already written in the [query](#query) class).

Then it implements the Handle method, where the actual logic happens (not really in this case).

The above class just looks for the user and returns it, otherwise if it's null it returns an error.

## Creating a New Endpoint

### Api

Inside the **Api**, we will create the following:

- a controller
- contracts for request and response
- mapping for contracts
- endpoint inside controller

#### Controller

Usually, there is one controller per aggregate. Also, if it is already created, go to the next chapter.

Under the package **Controllers**, we can create the following class:

```csharp
public sealed class ProjectController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator) {
    
}
```

The class will inherit from the **Api Controller** class, which requires a **mapper** and a **mediator**,
that are injected.

#### Contracts

Next, we need two classes that will define the **request** and the **response** of the new endpoint.

```csharp
public sealed record CreateProjectRequest(
    Guid UserId,
    string Name);
```

As already mentioned in the previous chapters,
it should match the naming and types of the command that it will be converted to.

And the response will return the properties of the newly created project:

```csharp
public sealed record CreateProjectResponse(
    Guid Id,
    string Name);
```

#### Mapping

The Next step would be to create the mapping for the request to command and for the result to response.

Under the package **Common/Mapping**, there should be a class per controller.

```csharp
public sealed class ProjectMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        ...
        CreateMappings(config);
        ...
    }
    
    ...
    
    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectRequest, CreateProjectCommand>();

        config.NewConfig<CreateProjectResult, CreateProjectResponse>()
            .Map(dest => dest.Id, src => src.Project.Id.Value)
            .Map(dest => dest.Name, src => src.Project.Name.Value);
    }
    
    ...
}
```

In the above example, the new class implements the interface **IRegister** from _Mapster_,
which requires the function **Register**, where we can create our functions for each endpoint to map.

As the naming and the types match for the request and command, there is no need for further mapping.

For the result, we need to explicitly map a few fields.
Also, notice that we can choose to omit a fields from being exposed outside the application (i.e., the created date
time).

#### Endpoint

Finally, we can write our new endpoint in the controller:

```csharp
public sealed class ProjectController(IMapper mapper, ISender mediator) : ApiController(mapper, mediator) {
    [HasPermission(Permissions.CreateProject)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateProjectCommand>(request);
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            createResult => Ok(Mapper.Map<CreateProjectResponse>(createResult)),
            Problem
        );
    }
}
```

The endpoint from above does the following:

- it checks whether the user that made the request has permission to create a project (not covered here)
- maps the request to a command
- it sends the command and captures the result
- then, whether it is a result or an error, it maps it to a response or a **Problem** response
