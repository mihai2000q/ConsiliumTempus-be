namespace ConsiliumTempus.Api.Common.Cors;

internal static class CorsPolicies
{
    public const string Policy = "FrontendPolicy";

    public static readonly string[] Methods =
    [
        "GET",
        "POST",
        "PUT",
        "PATCH",
        "DELETE"
    ];
}