using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Workspace.GetOverview;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetOverviewWorkspaceResponse(
    string Description);