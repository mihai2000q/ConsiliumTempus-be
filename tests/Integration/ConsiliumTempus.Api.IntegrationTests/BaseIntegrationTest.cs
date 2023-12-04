namespace ConsiliumTempus.Api.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<ConsiliumTempusWebApplicationFactory>, IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    
    protected readonly HttpClient Client;

    protected BaseIntegrationTest(ConsiliumTempusWebApplicationFactory factory)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        Client = factory.HttpClient;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}