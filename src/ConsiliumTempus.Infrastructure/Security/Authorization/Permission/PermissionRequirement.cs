using ConsiliumTempus.Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Permission;

public sealed record PermissionRequirement(Permissions Permission) : IAuthorizationRequirement;