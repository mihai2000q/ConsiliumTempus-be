using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Infrastructure.Authorization.Permission;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; set; } = permission;
}