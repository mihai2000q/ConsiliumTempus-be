using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsiliumTempus.Api.IntegrationTests.Core;

public abstract class BaseIntegrationTest : IAsyncLifetime
{
    private readonly ITestData? _testData;
    private readonly Func<Task> _resetDatabase;
    private readonly bool _defaultUsers;

    protected readonly AppHttpClient Client;
    protected readonly IDbContextFactory<ConsiliumTempusDbContext> DbContextFactory;
    protected readonly JwtSettings JwtSettings = new();

    protected BaseIntegrationTest(
        WebAppFactory factory,
        ITestData? testData = null,
        bool defaultUsers = true)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        _testData = testData;
        _defaultUsers = defaultUsers;
        Client = factory.CreateAppClient();
        DbContextFactory = factory.Services.GetRequiredService<IDbContextFactory<ConsiliumTempusDbContext>>();
        factory.Services.GetRequiredService<IConfiguration>().Bind(JwtSettings.SectionName, JwtSettings);
    }

    public async Task InitializeAsync()
    {
        if (_defaultUsers)
        {
            // ReSharper disable once CoVariantArrayConversion
            await AddTestData([DefaultUsers.Users]);
            Client.UseCustomToken();
        }

        if (_testData != null) await AddTestData(_testData.GetDataCollections());
    }

    public async Task DisposeAsync()
    {
        await _resetDatabase();
    }

    private async Task AddTestData(IEnumerable<object[]> data)
    {
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        foreach (var d in data)
        {
            await dbContext.AddRangeAsync(d);
        }
        await dbContext.SaveChangesAsync();
    }
}