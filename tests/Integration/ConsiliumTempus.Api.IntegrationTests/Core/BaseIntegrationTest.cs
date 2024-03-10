using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Core;

public abstract class BaseIntegrationTest : IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    private readonly string? _testDataDirectory;
    private Func<string> GetTestDataDirectoryPath => () => SetupConstants.TestDataDirectoryPath + _testDataDirectory;
    private readonly bool _defaultUsers;

    protected readonly ITestOutputHelper TestOutputHelper;
    protected readonly AppHttpClient Client;
    protected readonly IDbContextFactory<ConsiliumTempusDbContext> DbContextFactory;
    protected readonly JwtSettings JwtSettings = new();

    protected BaseIntegrationTest(
        WebAppFactory factory,
        ITestOutputHelper testOutputHelper,
        string? testDataDirectory = null,
        bool defaultUsers = true)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        _testDataDirectory = testDataDirectory;
        _defaultUsers = defaultUsers;
        Client = factory.CreateAppClient();
        TestOutputHelper = testOutputHelper;
        DbContextFactory = factory.Services.GetRequiredService<IDbContextFactory<ConsiliumTempusDbContext>>();
        factory.Services.GetRequiredService<IConfiguration>().Bind(JwtSettings.SectionName, JwtSettings);
    }

    public async Task InitializeAsync()
    {
        if (_defaultUsers)
        {
            await AddTestData(SetupConstants.DefaultUsersFilePath);
            Client.UseCustomToken();
        }

        if (_testDataDirectory != null)
        {
            var files = Directory.GetFiles(GetTestDataDirectoryPath()).Order();
            foreach (var file in files)
            {
                await AddTestData(file);
            }
        }
    }

    public async Task DisposeAsync()
    {
        await _resetDatabase();
    }

    private async Task AddTestData(string path)
    {
        var rawQueries = await File.ReadAllLinesAsync(path);
        var queries = ParseQueries(rawQueries);

        var dbContext = await DbContextFactory.CreateDbContextAsync();
        TestOutputHelper.WriteLine($"Importing data from: {path}");
        foreach (var query in queries)
        {
            await dbContext.Database.ExecuteSqlRawAsync(query);
        }
        TestOutputHelper.WriteLine($"Finished importing data from: {path}\n");
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