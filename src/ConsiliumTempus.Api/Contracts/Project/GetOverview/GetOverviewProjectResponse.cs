using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.GetOverview;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetOverviewProjectResponse(
    string Description);