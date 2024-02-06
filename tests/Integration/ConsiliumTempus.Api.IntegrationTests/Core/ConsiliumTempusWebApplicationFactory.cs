using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
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

namespace ConsiliumTempus.Api.IntegrationTests.Core;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class ConsiliumTempusWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string MsSqlImage = "mcr.microsoft.com/mssql/server:2022-latest";
    private const string DatabasePassword = "StrongPassword123";
    
    private readonly MsSqlContainer _dbContainer =
        new MsSqlBuilder()
            .WithImage(MsSqlImage)
            .WithPassword(DatabasePassword)
            .Build();

    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    public HttpClient HttpClient { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ConsiliumTempusDbContext>));
            services.AddDbContext<ConsiliumTempusDbContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));
            
            services.AddAuthentication(TestAuthHandler.AuthenticationSchema)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationSchema, _ => { });
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
            SchemasToInclude = ["dbo"]
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}