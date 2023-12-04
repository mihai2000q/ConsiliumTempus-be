using System.Data.Common;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Respawn;
using Testcontainers.MsSql;

namespace ConsiliumTempus.Api.IntegrationTests;

public class ConsiliumTempusWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer =
        new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("StrongPassword123")
            .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public HttpClient HttpClient { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ConsiliumTempusDbContext>));
            services.AddDbContext<ConsiliumTempusDbContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        // apply migrations to Test Database
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ConsiliumTempusDbContext>();
        await dbContext.Database.MigrateAsync();
        
        HttpClient = CreateClient();
        _dbConnection = new SqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = new[] { "dbo" }
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}