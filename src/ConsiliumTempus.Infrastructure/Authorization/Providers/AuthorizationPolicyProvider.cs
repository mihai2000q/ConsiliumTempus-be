using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Infrastructure.Authorization.IsOwner;
using ConsiliumTempus.Infrastructure.Authorization.Permission;
using ConsiliumTempus.Infrastructure.Authorization.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.Infrastructure.Authorization.Providers;

public sealed class AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy is not null) return policy;

        var builder = new AuthorizationPolicyBuilder();

        builder = policyName == Validate.Token.ToString()
            ? builder.AddRequirements(new TokenRequirement())
            : policyName == Validate.IsOwner.ToString()
                ? builder.AddRequirements(new IsOwnerRequirement())
                : builder.AddRequirements(new PermissionRequirement(policyName));

        return builder.Build();
    }
}