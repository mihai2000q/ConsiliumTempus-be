using ConsiliumTempus.Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.ProjectAuthorization;

internal sealed record ProjectAuthorizationRequirement(ProjectAuthorizationLevel AuthorizationLevel)
    : IAuthorizationRequirement;