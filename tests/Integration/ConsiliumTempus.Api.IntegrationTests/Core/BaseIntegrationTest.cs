using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using ConsiliumTempus.Api.IntegrationTests.Core.Authentication;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Core;

public abstract class BaseIntegrationTest : IClassFixture<WebAppFactory>, IAsyncLifetime
{
    private readonly ITokenProvider _tokenProvider;
    private readonly Func<Task> _resetDatabase;

    private readonly string? _testDataDirectory;
    private Func<string> GetTestDataDirectoryPath => () => SetupConstants.MockDirectoryPath + _testDataDirectory;
    private readonly bool _defaultUsers;

    protected readonly HttpClient Client;
    protected readonly IDbContextFactory<ConsiliumTempusDbContext> DbContextFactory;
    protected readonly ITestOutputHelper TestOutputHelper;
    protected readonly JwtSettings JwtSettings = new();

    protected BaseIntegrationTest(
        WebAppFactory factory,
        ITestOutputHelper testOutputHelper,
        string? testDataDirectory = null,
        bool defaultUsers = true)
    {
        _tokenProvider = factory.Services.GetRequiredService<ITokenProvider>();
        _resetDatabase = factory.ResetDatabaseAsync;
        _testDataDirectory = testDataDirectory;
        _defaultUsers = defaultUsers;
        Client = factory.HttpClient;
        TestOutputHelper = testOutputHelper;
        DbContextFactory = factory.Services.GetRequiredService<IDbContextFactory<ConsiliumTempusDbContext>>();
        factory.Services.GetRequiredService<IConfiguration>()
            .Bind(JwtSettings.SectionName, JwtSettings);
    }

    public async Task InitializeAsync()
    {
        if (_defaultUsers)
        {
            await AddTestData(SetupConstants.DefaultUsersFilePath);
            UseCustomToken();
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
        ResetToken();
        await _resetDatabase();
    }

    protected void UseCustomToken(string? email = null)
    {
        UseToken(GetToken(email));
    }

    protected void UseInvalidToken()
    {
        UseToken(GetInvalidToken());
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

    private void UseToken(JwtSecurityToken securityToken)
    {
        _tokenProvider.SetToken(securityToken);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            TestAuthHandler.AuthenticationSchema,
            Utils.Token.SecurityTokenToStringToken(securityToken));
    }

    private void ResetToken()
    {
        _tokenProvider.SetToken(null);
        Client.DefaultRequestHeaders.Authorization = null;
    }

    private JwtSecurityToken GetToken(string? email = null)
    {
        var dbContext = DbContextFactory.CreateDbContext();
        var user = email is null
            ? dbContext.Users.FirstOrDefault()
            : dbContext.Users.SingleOrDefault(u => u.Credentials.Email == email.ToLower());

        if (user is null) throw new Exception("There is no user with that email");

        return Utils.Token.GenerateValidToken(user, JwtSettings);
    }

    private JwtSecurityToken GetInvalidToken()
    {
        return Utils.Token.GenerateInvalidToken(JwtSettings);
    }
}