using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Infrastructure.Authorization.Permission;

public sealed record PermissionRequirement(string Permission) : IAuthorizationRequirement;