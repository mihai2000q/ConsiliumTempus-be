﻿using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Conflict(
            "Authentication.InvalidCredentials",
            "Invalid Credentials");
    }
}