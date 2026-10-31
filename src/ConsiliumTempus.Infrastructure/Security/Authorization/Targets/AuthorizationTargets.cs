using Microsoft.AspNetCore.Http;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Targets;

/// <summary>
/// The single place that knows where an endpoint carries the id of the entity it targets.
/// <para>
/// By convention, an endpoint targets the entity of its own controller, through the property <c>id</c>,
/// which is on the route for a <c>GET</c> or <c>DELETE</c>, and on the body for a <c>POST</c>, <c>PATCH</c> or <c>PUT</c>.
/// The endpoints that work through a parent entity instead (creating and listing) are the exceptions below.
/// </para>
/// The position of the id comes from the endpoint, never from the content of the request, because it has to be
/// the one that the model binding is going to use. An endpoint that cannot be resolved is an error, never a pass.
/// </summary>
internal static class AuthorizationTargets
{
    /// <summary>
    /// The controllers that have authorized endpoints, named as the routing names them
    /// </summary>
    private enum Controller
    {
        CustomFieldSetup,
        Project,
        ProjectSprint,
        ProjectTask,
        Workspace
    }

    /// <summary>
    /// The actions that are exceptions to the convention, named as the routing names them
    /// </summary>
    private enum Action
    {
        GetCollection,
        GetCollectionFromWorkspace,
        GetCollectionFromProject,
        GetInvitations,
        Create,
        CreateOnWorkspace,
        CreateOnProject,
        AddToProject,
        RemoveFromProject
    }

    /// <summary>
    /// The endpoints that are not about an entity of their own controller
    /// </summary>
    private static readonly Dictionary<(Controller Controller, Action Action), TargetId> ParentTargets = new()
    {
        [(Controller.CustomFieldSetup, Action.GetCollectionFromWorkspace)] =
            new TargetId(TargetKind.Workspace, IdLocation.Route, Qualified: true),
        [(Controller.CustomFieldSetup, Action.GetCollectionFromProject)] =
            new TargetId(TargetKind.Project, IdLocation.Route, Qualified: true),
        [(Controller.CustomFieldSetup, Action.CreateOnWorkspace)] =
            new TargetId(TargetKind.Workspace, IdLocation.Body, Qualified: true),
        [(Controller.CustomFieldSetup, Action.CreateOnProject)] =
            new TargetId(TargetKind.Project, IdLocation.Body, Qualified: true),
        [(Controller.Project, Action.GetCollection)] =
            new TargetId(TargetKind.Workspace, IdLocation.Query, Qualified: true),
        [(Controller.Project, Action.Create)] =
            new TargetId(TargetKind.Workspace, IdLocation.Body, Qualified: true),
        [(Controller.ProjectSprint, Action.GetCollection)] =
            new TargetId(TargetKind.Project, IdLocation.Query, Qualified: true),
        [(Controller.ProjectSprint, Action.Create)] =
            new TargetId(TargetKind.Project, IdLocation.Body, Qualified: true),
        [(Controller.ProjectTask, Action.GetCollection)] =
            new TargetId(TargetKind.ProjectStage, IdLocation.Query, Qualified: true),
        [(Controller.ProjectTask, Action.Create)] =
            new TargetId(TargetKind.ProjectStage, IdLocation.Body, Qualified: true),
        [(Controller.Workspace, Action.GetInvitations)] =
            new TargetId(TargetKind.Workspace, IdLocation.Query, Qualified: true)
    };

    /// <summary>
    /// The endpoints that, for the project authorization, are about the project, although the rest of
    /// the authorization is about another entity
    /// </summary>
    private static readonly Dictionary<(Controller Controller, Action Action), TargetId> ProjectTargets = new()
    {
        [(Controller.CustomFieldSetup, Action.AddToProject)] =
            new TargetId(TargetKind.Project, IdLocation.Body, Qualified: true),
        [(Controller.CustomFieldSetup, Action.RemoveFromProject)] =
            new TargetId(TargetKind.Project, IdLocation.Route, Qualified: true)
    };

    /// <summary>
    /// The entity targeted by the endpoint of the request
    /// </summary>
    internal static TargetId For(HttpRequest request)
    {
        return FindTarget(ParentTargets, request) ?? OwnTarget(request);
    }

    /// <summary>
    /// The entity targeted by the endpoint of the request, as far as the project authorization is concerned
    /// </summary>
    internal static TargetId ForProject(HttpRequest request)
    {
        return FindTarget(ProjectTargets, request) ?? For(request);
    }

    private static TargetId? FindTarget(
        Dictionary<(Controller Controller, Action Action), TargetId> targets,
        HttpRequest request)
    {
        return GetRouteValue<Controller>(request, "controller") is { } controller
               && GetRouteValue<Action>(request, "action") is { } action
               && targets.TryGetValue((controller, action), out var target)
            ? target
            : null;
    }

    /// <summary>
    /// The value of the route that is one of the names of the enum (and not a number, which would also parse to it)
    /// </summary>
    private static T? GetRouteValue<T>(HttpRequest request, string key)
        where T : struct, Enum
    {
        var name = request.RouteValues[key]?.ToString();
        return name is not null && Enum.IsDefined(typeof(T), name) ? Enum.Parse<T>(name) : null;
    }

    private static TargetId OwnTarget(HttpRequest request)
    {
        var kind = GetRouteValue<Controller>(request, "controller") switch
        {
            Controller.CustomFieldSetup => TargetKind.CustomFieldSetup,
            Controller.Project => TargetKind.Project,
            Controller.ProjectSprint => TargetKind.ProjectSprint,
            Controller.ProjectTask => TargetKind.ProjectTask,
            Controller.Workspace => TargetKind.Workspace,
            _ => throw new InvalidOperationException(
                $"The controller {request.RouteValues["controller"]} has no authorization target")
        };

        var method = request.Method;
        if (HttpMethods.IsGet(method) || HttpMethods.IsDelete(method)) return new TargetId(kind, IdLocation.Route);
        if (HttpMethods.IsPatch(method) || HttpMethods.IsPost(method) || HttpMethods.IsPut(method))
            return new TargetId(kind, IdLocation.Body);

        throw new InvalidOperationException($"The HTTP method '{method}' has no authorization target");
    }
}
