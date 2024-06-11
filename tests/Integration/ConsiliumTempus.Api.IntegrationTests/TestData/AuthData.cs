using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Common.IntegrationTests.Authentication;
using ConsiliumTempus.Common.IntegrationTests.User;
using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.TestData;

internal class AuthData : ITestData
{
    public IEnumerable<IEnumerable<object>> GetDataCollections()
    {
        return
        [
            Users,
            RefreshTokens
        ];
    }

    public static UserAggregate[] Users { get; } =
    [
        UserFactory.Create(
            "michaelj@gmail.com",
            "$2a$13$3aq7qz/2sY2zPApVzUWhVeWoZZbk3bZgWhH96aUi7ElS.DIoitRNS",
            "Michael",
            "Jordan",
            "Pro Basketball Player")
    ];

    public static RefreshToken[] RefreshTokens { get; } =
    [
        RefreshTokenFactory.Create(
            Users[0],
            history: [RefreshTokenHistoryFactory.Create(Guid.Parse("90000000-9000-0000-0000-900000000000"))]),
        RefreshTokenFactory.Create(
            Users[0],
            history: [
                RefreshTokenHistoryFactory.Create(Guid.Parse("90000000-9000-0000-0000-900000000000")),
                RefreshTokenHistoryFactory.Create(Guid.Parse("90000000-9000-0000-0000-900000000001"))
            ]),
        RefreshTokenFactory.Create(
            Users[0],
            isInvalidated: true,
            history: [RefreshTokenHistoryFactory.Create(Guid.Parse("90000000-9000-0000-0000-900000000000"))]),
        RefreshTokenFactory.Create(
            Users[0],
            expiryDateTime: DateTime.UtcNow.AddMilliseconds(-1),
            history: [RefreshTokenHistoryFactory.Create(Guid.Parse("90000000-9000-0000-0000-900000000000"))]),
        RefreshTokenFactory.Create(
            Users[0],
            history: [RefreshTokenHistoryFactory.Create(Guid.Parse("12345678-1234-1234-1234-123456789123"))]),
    ];
}