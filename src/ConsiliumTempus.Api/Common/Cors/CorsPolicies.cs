namespace ConsiliumTempus.Api.Common.Cors;

internal static class CorsPolicies
{
    internal static class Frontend
    {
        public const string Policy = "FrontendPolicy";
        public const string Origin = "http://localhost:8123";

        public static readonly string[] Methods =
        [
            "GET",
            "POST",
            "PUT",
            "DELETE"
        ];

        public static readonly string[] Headers =
        [
            "Authorization",
            "Content-Type"
        ];
    }
}