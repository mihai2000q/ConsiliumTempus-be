using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Token;

public sealed record TokenRequirement : IAuthorizationRequirement;