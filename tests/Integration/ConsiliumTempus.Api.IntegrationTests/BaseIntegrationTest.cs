using ConsiliumTempus.Infrastructure.Authentication;

namespace ConsiliumTempus.Api.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<ConsiliumTempusWebApplicationFactory>, IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    
    protected readonly HttpClient Client;

    protected readonly JwtSettings JwtSettings;

    protected BaseIntegrationTest(ConsiliumTempusWebApplicationFactory factory)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        Client = factory.HttpClient;
        JwtSettings = factory.JwtSettings;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}