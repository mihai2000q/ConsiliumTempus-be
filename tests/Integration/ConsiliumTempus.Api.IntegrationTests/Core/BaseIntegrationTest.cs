using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using ConsiliumTempus.Api.IntegrationTests.Core.Authentication;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Infrastructure.Authentication;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Core;

public abstract class BaseIntegrationTest : IClassFixture<ConsiliumTempusWebApplicationFactory>, IAsyncLifetime
{
    private readonly ITokenProvider _tokenProvider;
    private readonly ConsiliumTempusDbContext _dbContext;
    private readonly Func<Task> _resetDatabase;

    private readonly string? _testDataDirectory;
    private Func<string> GetTestDataDirectoryPath => () => Constants.MockDirectoryPath + _testDataDirectory;
    private readonly bool _defaultUsers;

    protected readonly HttpClient Client;
    protected readonly ITestOutputHelper TestOutputHelper;
    protected readonly JwtSettings JwtSettings = new();

    protected BaseIntegrationTest(
        ConsiliumTempusWebApplicationFactory factory,
        ITestOutputHelper testOutputHelper,
        string? testDataDirectory = null,
        bool defaultUsers = true)
    {
        _tokenProvider = factory.Services.GetRequiredService<ITokenProvider>();
        _dbContext = factory.Services.GetRequiredService<ConsiliumTempusDbContext>();
        _resetDatabase = factory.ResetDatabaseAsync;
        _testDataDirectory = testDataDirectory;
        _defaultUsers = defaultUsers;
        Client = factory.HttpClient;
        TestOutputHelper = testOutputHelper;
        factory.Services.GetRequiredService<IConfiguration>()
            .Bind(JwtSettings.SectionName, JwtSettings);
    }

    public async Task InitializeAsync()
    {
        if (_defaultUsers)
        {
            await AddTestData(Constants.DefaultUsersFilePath);
            UseCustomToken();
        }

        if (_testDataDirectory != null)
        {
            foreach (var file in Directory.GetFiles(GetTestDataDirectoryPath()))
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

        foreach (var query in queries)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(query);
        }
        TestOutputHelper.WriteLine($"Imported data from: {path}");
    }

    private static IEnumerable<string> ParseQueries(IEnumerable<string> rawQueries)
    {
        return rawQueries.Where(line => !line.TrimStart().StartsWith("--"))
            .Select(l => l.Trim())
            .Aggregate((curr, next) => curr + next)
            .Split(";")
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(q => q.Replace("VALUES", " VALUES ") + ";");
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
        var user = email is null
            ? _dbContext.Users.FirstOrDefault()
            : _dbContext.Users.SingleOrDefault(u => u.Credentials.Email == email.ToLower());

        if (user is null) throw new Exception("There is no user with that email");

        return Utils.Token.CreateMock(user, JwtSettings);
    }

    private JwtSecurityToken GetInvalidToken()
    {
        return Utils.Token.CreateInvalidToken(JwtSettings);
    }
}