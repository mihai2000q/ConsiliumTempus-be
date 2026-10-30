using ConsiliumTempus.Infrastructure.Security.Authorization.Permission;
using ConsiliumTempus.Infrastructure.Security.Authorization.WorkspaceAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.NotFound;

/// <summary>
/// Answers with the same not found response that the endpoint gives when the resource does not exist,
/// whenever the authorization failed because the user has no relationship with the resource,
/// so that the existence of the resource is not revealed
/// </summary>
internal sealed class NotFoundAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    // Must match the key and the extension used by the Api when it builds the problem details of its errors
    private const string ErrorsItemKey = "Errors";

    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        var reason = authorizeResult.Forbidden
            ? authorizeResult.AuthorizationFailure?.FailureReasons
                .OfType<NotFoundAuthorizationFailureReason>()
                .OrderBy(r => GetPriority(r.Handler))
                .FirstOrDefault()
            : null;

        if (reason is null)
        {
            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
            return;
        }

        context.Items[ErrorsItemKey] = new List<ErrorOr.Error> { reason.Error };

        var problemDetails = context.RequestServices
            .GetRequiredService<ProblemDetailsFactory>()
            .CreateProblemDetails(
                context,
                StatusCodes.Status404NotFound,
                reason.Error.Description);

        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsJsonAsync(
            problemDetails,
            options: null,
            contentType: "application/problem+json");
    }

    /// <summary>
    /// When the request has more than one resource, the outermost one (the one that is checked first
    /// by the handlers of the endpoint) decides which not found error is returned
    /// </summary>
    private static int GetPriority(IAuthorizationHandler handler)
    {
        return handler switch
        {
            PermissionAuthorizationHandler => 0,
            WorkspaceAuthorizationHandler => 1,
            _ => 2
        };
    }
}