using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.IsOwner;

public sealed record IsOwnerRequirement : IAuthorizationRequirement;