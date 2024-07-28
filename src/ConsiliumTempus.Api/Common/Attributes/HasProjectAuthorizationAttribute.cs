using ConsiliumTempus.Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Api.Common.Attributes;

public sealed class HasProjectAuthorizationAttribute(ProjectAuthorizationLevel projectAuthorizationLevel) 
    : AuthorizeAttribute(projectAuthorizationLevel.ToString());