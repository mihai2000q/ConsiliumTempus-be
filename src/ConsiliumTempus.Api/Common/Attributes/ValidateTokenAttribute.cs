using ConsiliumTempus.Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ConsiliumTempus.Api.Common.Attributes;

public sealed class ValidateTokenAttribute(Validate policy = Validate.Token)
    : AuthorizeAttribute(policy.ToString());

