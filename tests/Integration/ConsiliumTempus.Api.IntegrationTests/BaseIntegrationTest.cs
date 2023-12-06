using ConsiliumTempus.Infrastructure.Authentication;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsiliumTempus.Api.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<ConsiliumTempusWebApplicationFactory>, IAsyncLifetime
{
    private readonly ConsiliumTempusDbContext _dbContext;
    private readonly Func<Task> _resetDatabase;
    
    private readonly string? _testDataFilename;
    private Func<string> GetTestDataFilePath => () => $"../../../MockData/{_testDataFilename}.sql";

    protected readonly HttpClient Client;
    protected readonly JwtSettings JwtSettings = new();

    protected BaseIntegrationTest(ConsiliumTempusWebApplicationFactory factory, string? testDataFilename = null)
    {
        _dbContext = factory.Services.GetRequiredService<ConsiliumTempusDbContext>();
        _resetDatabase = factory.ResetDatabaseAsync;
        _testDataFilename = testDataFilename;
        Client = factory.HttpClient;
        factory.Services.GetRequiredService<IConfiguration>()
            .Bind(JwtSettings.SectionName, JwtSettings);
    }

    public async Task InitializeAsync()
    {
        if (_testDataFilename != null)
        {
            await AddTestData();
        }
    }

    public async Task DisposeAsync()
    {
        await _resetDatabase();
    }

    private async Task AddTestData()
    {
        var rawQueries = await File.ReadAllLinesAsync(GetTestDataFilePath());
        var queries = ParseQueries(rawQueries);

        foreach (var query in queries)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(query);
        }
    }

    private static IEnumerable<string> ParseQueries(IEnumerable<string> rawQueries)
    {
        return rawQueries.Where(line => !line.TrimStart().StartsWith("--"))
            .Select(l => l.Trim())
            .Aggregate((curr, next) => curr + next)
            .Split(";")
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(q => q + ";");
    }
}