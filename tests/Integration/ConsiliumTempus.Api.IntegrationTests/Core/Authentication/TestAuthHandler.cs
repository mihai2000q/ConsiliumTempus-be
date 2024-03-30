using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.Api.IntegrationTests.Core.Authentication;

public class TestAuthHandler(
    TokenProvider tokenProvider,
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private const string AuthenticationType = "Test";
    
    public const string AuthenticationSchema = "TestSchema";

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = tokenProvider.GetToken();
        var identity = new ClaimsIdentity(token?.Claims ?? Array.Empty<Claim>(), AuthenticationType);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationSchema);

        var result = AuthenticateResult.Success(ticket);

        return await Task.FromResult(result);
    }
}