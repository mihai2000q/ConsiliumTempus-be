using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Infrastructure.Authorization.IsOwner;

public sealed record IsOwnerRequirement : IAuthorizationRequirement;