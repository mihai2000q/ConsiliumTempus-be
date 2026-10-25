using ConsiliumTempus.Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.WorkspaceAuthorization;

internal sealed record WorkspaceAuthorizationRequirement(WorkspaceAuthorizationLevel AuthorizationLevel)
    : IAuthorizationRequirement;