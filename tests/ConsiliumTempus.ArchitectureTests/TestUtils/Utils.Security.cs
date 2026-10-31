using System.Reflection;
using System.Text.RegularExpressions;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using ConsiliumTempus.Infrastructure.Security.Authorization.Targets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace ConsiliumTempus.ArchitectureTests.TestUtils;

public static partial class Utils
{
    internal static class Security
    {
        /// <summary>
        /// The endpoints (<c>Controller.Action</c>) whose id is optional on purpose.
        /// With no id, there is no entity to authorize against, so the request is left to its validation,
        /// which is why every optional id has to be chosen here, instead of slipping in unnoticed.
        /// </summary>
        private static readonly HashSet<string> OptionalIdEndpoints =
        [
            "Project.GetCollection", // lists the projects of the user, filtered by the workspace when there is one
            "Workspace.GetInvitations" // validated to have either the workspace or the sender, never none
        ];

        internal sealed record Endpoint(string Controller, string Action, string HttpMethod, MethodInfo Method)
        {
            public List<ParameterInfo> RequestParameters => Method
                .GetParameters()
                .Where(p => p.ParameterType != typeof(CancellationToken))
                .ToList();

            public IEnumerable<string> Policies => Method
                .GetCustomAttributes<AuthorizeAttribute>()
                .Concat(Method.DeclaringType!.GetCustomAttributes<AuthorizeAttribute>())
                .Select(a => a.Policy)
                .OfType<string>();

            public IEnumerable<string> RouteTemplates => Method
                .GetCustomAttributes(inherit: true)
                .Concat(Method.DeclaringType!.GetCustomAttributes(inherit: true))
                .OfType<IRouteTemplateProvider>()
                .Select(t => t.Template)
                .OfType<string>();

            public HttpRequest ToRequest()
            {
                var context = new DefaultHttpContext();
                context.Request.Method = HttpMethod;
                context.Request.RouteValues =
                    new RouteValueDictionary { ["controller"] = Controller, ["action"] = Action };
                return context.Request;
            }

            public override string ToString() => $"{Controller}.{Action} ({HttpMethod})";
        }

        /// <summary>
        /// The name that the routing gives to a controller
        /// </summary>
        internal static string GetControllerName(Type controller) =>
            controller.Name.EndsWith("Controller") ? controller.Name[..^"Controller".Length] : controller.Name;

        internal static List<Endpoint> GetEndpoints()
        {
            return typeof(ApiController).Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(ApiController)))
                .SelectMany(controller => controller
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.DeclaringType!.IsSubclassOf(typeof(ApiController)))
                    .SelectMany(m => m
                        .GetCustomAttributes(inherit: true)
                        .OfType<IActionHttpMethodProvider>()
                        .SelectMany(a => a.HttpMethods)
                        .Distinct()
                        .Select(httpMethod => new Endpoint(
                            GetControllerName(controller),
                            GetActionName(m),
                            httpMethod,
                            m))))
                .ToList();
        }

        /// <summary>
        /// The endpoints that an authorization requirement of that type applies to, either on the action or on
        /// its controller, found the way the application finds it, through the policy of the attributes
        /// </summary>
        internal static async Task<List<Endpoint>> GetEndpointsRequiring<TRequirement>()
            where TRequirement : IAuthorizationRequirement
        {
            var policyProvider = new AuthorizationPolicyProvider(Options.Create(new AuthorizationOptions()));

            var endpoints = new List<Endpoint>();
            foreach (var endpoint in GetEndpoints())
            foreach (var policyName in endpoint.Policies)
            {
                var policy = await policyProvider.GetPolicyAsync(policyName);
                if (policy is null || !policy.Requirements.OfType<TRequirement>().Any()) continue;

                endpoints.Add(endpoint);
                break;
            }

            return endpoints;
        }

        /// <summary>
        /// What is wrong, for each endpoint, between the entity that the authorization targets and what the request
        /// of the endpoint really binds. When <paramref name="supportedKinds"/> is not set, every kind is supported.
        /// </summary>
        internal static List<string> GetMismatches(
            List<Endpoint> endpoints,
            Func<HttpRequest, TargetId> getTarget,
            TargetKind[]? supportedKinds = null)
        {
            if (endpoints.Count == 0) return ["no authorized endpoints were found"];

            var mismatches = new List<string>();
            foreach (var endpoint in endpoints)
            {
                if (GetMismatch(endpoint, getTarget, supportedKinds) is { } mismatch)
                    mismatches.Add($"{endpoint}: {mismatch}");
            }

            return mismatches;
        }

        private static string? GetMismatch(
            Endpoint endpoint,
            Func<HttpRequest, TargetId> getTarget,
            TargetKind[]? supportedKinds)
        {
            TargetId target;
            try
            {
                target = getTarget(endpoint.ToRequest());
            }
            catch (InvalidOperationException e)
            {
                return e.Message;
            }

            if (supportedKinds is not null && !supportedKinds.Contains(target.Kind))
                return $"the handler cannot look up a {target.Kind}";

            if (endpoint.RequestParameters is not [var parameter])
                return $"expected a single request parameter, but found {endpoint.RequestParameters.Count}";

            var bound = GetBoundProperties(parameter)
                .Where(p => string.Equals(p.Name, target.Property, StringComparison.OrdinalIgnoreCase))
                .ToList();
            if (bound.Count != 1)
                return $"expected a single request property bound as '{target.Property}', but found {bound.Count}";

            var (name, type, source) = bound[0];
            if ((Nullable.GetUnderlyingType(type) ?? type) != typeof(Guid))
                return $"'{name}' is a {type.Name}, not a Guid";

            var isOptional = Nullable.GetUnderlyingType(type) is not null;
            var isAllowedToBeOptional = OptionalIdEndpoints.Contains($"{endpoint.Controller}.{endpoint.Action}");
            if (isOptional && !isAllowedToBeOptional)
                return $"'{name}' is optional, which skips the authorization when it is absent";
            if (!isOptional && isAllowedToBeOptional)
                return $"'{name}' is not optional, so the endpoint should not be one of the {nameof(OptionalIdEndpoints)}";

            var location = source == BindingSource.Path ? IdLocation.Route
                : source == BindingSource.Query ? IdLocation.Query
                : source == BindingSource.Body ? IdLocation.Body
                : (IdLocation?)null;
            if (location is null)
                return $"'{name}' is bound from {source.DisplayName}, which the authorization does not read";
            if (location != target.Location)
                return $"'{name}' is bound from the {location}, not from the {target.Location}";

            if (location == IdLocation.Route && !endpoint.RouteTemplates.Any(t => HasPlaceholder(t, name)))
                return $"the route has no '{{{name}}}'";

            return null;
        }

        /// <summary>
        /// The properties of the request, with the name and the source that the model binding gives them:
        /// the attributes can be on the property, on the parameter of the constructor of a record
        /// or, for the source, on the parameter of the action
        /// </summary>
        private static IEnumerable<(string Name, Type Type, BindingSource Source)> GetBoundProperties(
            ParameterInfo parameter)
        {
            var constructor = parameter.ParameterType
                .GetConstructors()
                .MaxBy(c => c.GetParameters().Length);
            var parameterSource = parameter
                .GetCustomAttributes(inherit: true)
                .OfType<IBindingSourceMetadata>()
                .Select(a => a.BindingSource)
                .FirstOrDefault();

            foreach (var property in parameter.ParameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attributes = property
                    .GetCustomAttributes(inherit: true)
                    .Concat(constructor?
                        .GetParameters()
                        .FirstOrDefault(p => string.Equals(p.Name, property.Name, StringComparison.OrdinalIgnoreCase))?
                        .GetCustomAttributes(inherit: true) ?? [])
                    .ToList();

                var name = attributes.OfType<IModelNameProvider>().Select(a => a.Name).FirstOrDefault(n => n is not null)
                           ?? property.Name;
                var source = attributes.OfType<IBindingSourceMetadata>().Select(a => a.BindingSource).FirstOrDefault()
                             ?? parameterSource
                             ?? BindingSource.Body;

                yield return (name, property.PropertyType, source!);
            }
        }

        /// <summary>
        /// The name that the routing gives to an action: the suffix <c>Async</c> is not part of it
        /// </summary>
        private static string GetActionName(MethodInfo action) =>
            action.GetCustomAttribute<ActionNameAttribute>()?.Name
            ?? (action.Name.EndsWith("Async") ? action.Name[..^"Async".Length] : action.Name);

        private static bool HasPlaceholder(string template, string name) =>
            Regex.IsMatch(template, $@"\{{\*{{0,2}}{Regex.Escape(name)}(?=[:?=}}])[^}}]*\}}|\{{\*{{0,2}}{Regex.Escape(name)}\}}",
                RegexOptions.IgnoreCase);
    }
}
