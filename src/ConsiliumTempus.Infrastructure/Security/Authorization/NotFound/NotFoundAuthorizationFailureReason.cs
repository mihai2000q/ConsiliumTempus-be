using ErrorOr;
using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.NotFound;

/// <summary>
/// The resource exists, but the user has no relationship with it (e.g. is not a collaborator of its workspace),
/// so the user should not be able to tell it apart from a resource that does not exist
/// </summary>
internal sealed class NotFoundAuthorizationFailureReason(IAuthorizationHandler handler, Error error)
    : AuthorizationFailureReason(handler, error.Description)
{
    public Error Error { get; } = error;
}