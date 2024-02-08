using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Api.Common.Attributes;

public sealed class HasPermissionAttribute(Permissions permission) 
    : AuthorizeAttribute(permission.ToString());