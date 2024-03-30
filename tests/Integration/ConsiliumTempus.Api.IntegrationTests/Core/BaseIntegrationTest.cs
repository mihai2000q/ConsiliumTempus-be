using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsiliumTempus.Api.IntegrationTests.Core;

public abstract class BaseIntegrationTest : IAsyncLifetime
{
    private readonly ITestData _testData;
    private readonly Func<Task> _resetDatabase;
    private readonly bool _isAnonymous;

    protected readonly AppHttpClient Client;
    protected readonly IDbContextFactory<ConsiliumTempusDbContext> DbContextFactory;
    protected readonly JwtSettings JwtSettings = new();

    protected BaseIntegrationTest(
        WebAppFactory factory,
        ITestData testData,
        bool isAnonymous = false)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        _testData = testData;
        _isAnonymous = isAnonymous;
        Client = factory.CreateAppClient();
        DbContextFactory = factory.Services.GetRequiredService<IDbContextFactory<ConsiliumTempusDbContext>>();
        factory.Services.GetRequiredService<IConfiguration>().Bind(JwtSettings.SectionName, JwtSettings);
    }

    public async Task InitializeAsync()
    {
        await AddTestData(_testData.GetDataCollections());

        if (!_isAnonymous) Client.UseCustomToken();
    }

    public async Task DisposeAsync()
    {
        await _resetDatabase();
    }

    private async Task AddTestData(IEnumerable<IEnumerable<object>> data)
    {
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        foreach (var d in data.SelectMany(x => x))
        {
            if (d is Membership membership) dbContext.Attach(membership.WorkspaceRole);
            await dbContext.AddAsync(d);
        }
        await dbContext.SaveChangesAsync();
    }
}