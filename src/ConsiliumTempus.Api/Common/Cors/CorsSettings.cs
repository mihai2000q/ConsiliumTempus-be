namespace ConsiliumTempus.Api.Common.Cors;

internal sealed class CorsSettings
{
    public const string SectionName = "CorsSettings";
    public string FrontendOrigin { get; init; } = string.Empty;
}
