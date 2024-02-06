using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Unauthorized(
            "Authentication.InvalidCredentials",
            "Invalid Credentials");
        
        public static Error InvalidToken => Error.Unauthorized(
            "Jwt.InvalidToken",
            "Invalid Token");
    }
}