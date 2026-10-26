using ConsiliumTempus.Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Permission;

internal sealed record PermissionRequirement(Permissions Permission) : IAuthorizationRequirement;