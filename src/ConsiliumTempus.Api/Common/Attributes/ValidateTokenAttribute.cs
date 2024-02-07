using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Api.Common.Attributes;

public sealed class ValidateTokenAttribute(ValidateTokenEnum policy = ValidateTokenEnum.ValidateToken)
    : AuthorizeAttribute(policy.ToString());

public enum ValidateTokenEnum
{
    ValidateToken
}