using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ConsiliumTempus.Api.IntegrationTests.Core.Authentication;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using ConsiliumTempus.Infrastructure.Security.Authentication;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Core;

public class AppHttpClient(
    HttpClient client,
    ITokenProvider tokenProvider,
    IDbContextFactory<ConsiliumTempusDbContext> dbContextFactory,
    JwtSettings jwtSettings)
{
    public Task<HttpResponseMessage> Get(string requestUri)
    {
        return client.GetAsync(requestUri);
    }
    
    public Task<HttpResponseMessage> Post<TRequest>(string requestUri, TRequest request)
    {
        return client.PostAsJsonAsync(requestUri, request);
    }
    
    public Task<HttpResponseMessage> Put<TRequest>(string requestUri, TRequest request)
    {
        return client.PutAsJsonAsync(requestUri, request);
    }
    
    public Task<HttpResponseMessage> Delete(string requestUri)
    {
        return client.DeleteAsync(requestUri);
    }
    
    public UserId UseCustomToken(string? email = null)
    {
        var res = GetToken(email);
        UseToken(res.Item1);
        return res.Item2;
    }

    public void UseInvalidToken()
    {
        UseToken(GetInvalidToken());
    }
    
    
    private void UseToken(JwtSecurityToken securityToken)
    {
        tokenProvider.SetToken(securityToken);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            TestAuthHandler.AuthenticationSchema,
            Utils.Token.SecurityTokenToStringToken(securityToken));
    }

    private (JwtSecurityToken, UserId) GetToken(string? email = null)
    {
        var dbContext = dbContextFactory.CreateDbContext();
        var user = email is null
            ? dbContext.Users.FirstOrDefault()
            : dbContext.Users.SingleOrDefault(u => u.Credentials.Email == email.ToLower());

        if (user is null) throw new Exception("There is no user with that email");

        return (Utils.Token.GenerateValidToken(user, jwtSettings), user.Id);
    }

    private JwtSecurityToken GetInvalidToken()
    {
        return Utils.Token.GenerateInvalidToken(jwtSettings);
    }
}