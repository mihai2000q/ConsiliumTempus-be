using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Permission;

public sealed record PermissionRequirement(string Permission) : IAuthorizationRequirement;