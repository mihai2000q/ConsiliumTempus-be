using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.UnitTests.Common.Entities;

public static class RefreshTokenFactory
{
    public static RefreshToken Create(
        string? jwtId = null,
        UserAggregate? user = null)
    {
        return RefreshToken.Create(
            jwtId ?? Guid.NewGuid().ToString(),
            user ?? UserFactory.Create());
    }
}