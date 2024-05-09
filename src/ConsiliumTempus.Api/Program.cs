using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api;
using ConsiliumTempus.Api.Common.Cors;
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
    app.UsePathBase(new PathString("/Api"));
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseCors(CorsPolicies.Frontend.Policy);
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public partial class Program;