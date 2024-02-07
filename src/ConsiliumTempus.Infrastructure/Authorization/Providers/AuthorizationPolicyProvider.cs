using ConsiliumTempus.Infrastructure.Authorization.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.Infrastructure.Authorization.Providers;

public class AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) 
    : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy is not null) return policy;

        var builder = new AuthorizationPolicyBuilder()
            .AddRequirements(new TokenRequirement());
        
        return builder.Build();
    }
}