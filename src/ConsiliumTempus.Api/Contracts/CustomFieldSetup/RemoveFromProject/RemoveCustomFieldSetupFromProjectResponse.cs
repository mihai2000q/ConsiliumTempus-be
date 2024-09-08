using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.RemoveFromProject;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record RemoveCustomFieldSetupFromProjectResponse(string Message);