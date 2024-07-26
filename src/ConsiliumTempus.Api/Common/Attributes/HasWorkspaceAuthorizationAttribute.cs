using ConsiliumTempus.Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Api.Common.Attributes;

public sealed class HasWorkspaceAuthorizationAttribute(WorkspaceAuthorizationLevel workspaceAuthorizationLevel) 
    : AuthorizeAttribute(workspaceAuthorizationLevel.ToString());