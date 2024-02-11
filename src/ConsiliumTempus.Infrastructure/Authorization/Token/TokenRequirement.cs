using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Infrastructure.Authorization.Token;

public sealed record TokenRequirement : IAuthorizationRequirement;