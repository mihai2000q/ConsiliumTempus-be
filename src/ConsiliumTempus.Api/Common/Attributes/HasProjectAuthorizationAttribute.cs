using ConsiliumTempus.Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Api.Common.Attributes;

internal sealed class HasProjectAuthorizationAttribute(ProjectAuthorizationLevel projectAuthorizationLevel)
    : AuthorizeAttribute(projectAuthorizationLevel.ToString());