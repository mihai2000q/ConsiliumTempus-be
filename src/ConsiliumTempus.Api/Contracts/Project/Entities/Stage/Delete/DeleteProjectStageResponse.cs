using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Delete;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record DeleteProjectStageResponse(string Message);