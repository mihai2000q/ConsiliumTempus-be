using ConsiliumTempus.Api;
using ConsiliumTempus.Application;
using ConsiliumTempus.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Logging
        .AddLogger(builder.Configuration);
    
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddPresentation();
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}

public partial class Program {  }