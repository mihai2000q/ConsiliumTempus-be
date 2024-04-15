using ConsiliumTempus.Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Api.Common.Attributes;

public sealed class HasPermissionAttribute(Permissions permission)
    : AuthorizeAttribute(permission.ToString());