namespace ConsiliumTempus.Api.Common.Cors;

public static class CorsPolicies
{
    public static class Frontend
    {
        public const string Policy = "FrontendPolicy";
        public const string Origin = "http://localhost:8123";
        public static readonly string[] Methods = [
            "GET", 
            "POST", 
            "PUT", 
            "DELETE"
        ];
        public static readonly string[] Headers = [
            "Authorization", 
            "Content-Type"
        ];
    }
    
}