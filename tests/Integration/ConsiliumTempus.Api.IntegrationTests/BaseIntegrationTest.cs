using System.Net.Http.Headers;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
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
    private static Func<string, string> GetTestDataFilePath => fileName => $"../../../MockData/{fileName}.sql";
    private readonly bool _defaultUsers;

    protected readonly HttpClient Client;
    protected readonly JwtSettings JwtSettings = new();

    protected BaseIntegrationTest(
        ConsiliumTempusWebApplicationFactory factory, 
        string? testDataFilename = null,
        bool defaultUsers = true)
    {
        _dbContext = factory.Services.GetRequiredService<ConsiliumTempusDbContext>();
        _resetDatabase = factory.ResetDatabaseAsync;
        _testDataFilename = testDataFilename;
        _defaultUsers = defaultUsers;
        Client = factory.HttpClient;
        factory.Services.GetRequiredService<IConfiguration>()
            .Bind(JwtSettings.SectionName, JwtSettings);
    }

    public async Task InitializeAsync()
    {
        if (_testDataFilename != null)
        {
            await AddTestData(GetTestDataFilePath(_testDataFilename));
        }

        if (_defaultUsers)
        {
            await AddTestData(GetTestDataFilePath("Users"));
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                TestAuthHandler.AuthenticationSchema, 
                GetToken());
        }
    }

    public async Task DisposeAsync()
    {
        await _resetDatabase();
    }

    protected string GetToken(string? email = null)
    {
        var user = email is null ? 
            _dbContext.Users.FirstOrDefault() :
            _dbContext.Users.FirstOrDefault(u => u.Credentials.Email == email);

        if (user is null) throw new Exception("There is no user with that email");
            
        var token = Utils.Token.CreateMock(user, JwtSettings);
        return $"{token}";
    }

    private async Task AddTestData(string path)
    {
        var rawQueries = await File.ReadAllLinesAsync(path);
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