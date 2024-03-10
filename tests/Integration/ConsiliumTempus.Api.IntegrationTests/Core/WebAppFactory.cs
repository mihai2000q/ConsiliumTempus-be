using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.IntegrationTests.Core.Authentication;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Respawn;
using Testcontainers.MsSql;

namespace ConsiliumTempus.Api.IntegrationTests.Core;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer =
        new MsSqlBuilder()
            .WithImage(SetupConstants.MsSqlImage)
            .WithPassword(SetupConstants.DatabasePassword)
            .Build();

    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(SetupConstants.Environment);
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ConsiliumTempusDbContext>));
            services.AddDbContext<ConsiliumTempusDbContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));
            services.AddDbContextFactory<ConsiliumTempusDbContext>();

            services.AddSingleton<ITokenProvider, TokenProvider>();

            services.AddAuthentication(auth =>
                {
                    auth.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationSchema;
                    auth.DefaultChallengeScheme = TestAuthHandler.AuthenticationSchema;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.AuthenticationSchema,
                    _ => { });
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

        _dbConnection = new SqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawner();
    }

    public AppHttpClient CreateAppClient()
    {
        using var scope = Services.CreateScope();

        var tokenProvider = scope.ServiceProvider.GetRequiredService<ITokenProvider>();
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ConsiliumTempusDbContext>>();
        var jwtSettings = new JwtSettings();
        scope.ServiceProvider.GetRequiredService<IConfiguration>().Bind(JwtSettings.SectionName, jwtSettings);

        return new AppHttpClient(CreateClient(), tokenProvider, dbContextFactory, jwtSettings);
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = ["dbo"],
            TablesToIgnore = SetupConstants.TablesToIgnore
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}