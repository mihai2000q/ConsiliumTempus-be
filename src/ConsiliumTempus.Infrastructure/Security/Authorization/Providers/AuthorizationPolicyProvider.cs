using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Infrastructure.Security.Authorization.Permission;
using ConsiliumTempus.Infrastructure.Security.Authorization.WorkspaceAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Providers;

public sealed class AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        await Task.CompletedTask;

        if (Enum.TryParse<WorkspaceAuthorizationLevel>(policyName, out var workspaceAuthorizationLevel))
            return new AuthorizationPolicyBuilder()
                .AddRequirements(new WorkspaceAuthorizationRequirement(workspaceAuthorizationLevel))
                .Build();

        return new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();
    }
}