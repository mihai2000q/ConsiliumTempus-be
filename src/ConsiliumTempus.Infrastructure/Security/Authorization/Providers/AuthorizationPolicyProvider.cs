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

        var builder = new AuthorizationPolicyBuilder();

        if (Enum.TryParse<WorkspaceAuthorizationLevel>(policyName, out var workspaceAuthorizationLevel))
            builder.AddRequirements(new WorkspaceAuthorizationRequirement(workspaceAuthorizationLevel));

        else if (Enum.TryParse<Permissions>(policyName, out var permission))
            builder.AddRequirements(new PermissionRequirement(permission));

        return builder.Build();
    }
}