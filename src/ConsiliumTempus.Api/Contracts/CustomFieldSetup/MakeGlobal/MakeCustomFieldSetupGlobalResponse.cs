using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.MakeGlobal;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record MakeCustomFieldGlobalResponse(string Message);