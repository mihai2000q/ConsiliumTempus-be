using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestFactory.Domain;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.TestData;

internal class AuthData : ITestData
{
    public IEnumerable<object[]> GetDataCollections()
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
            new Guid("90000000-9000-0000-0000-900000000000"),
            Users.First()),
        RefreshTokenFactory.Create(
            new Guid("90000000-9000-0000-0000-900000000000"),
            Users.First(),
            isInvalidated: true),
        RefreshTokenFactory.Create(
            new Guid("90000000-9000-0000-0000-900000000000"),
            Users.First(),
            expiryDateTime: DateTime.UtcNow.AddSeconds(-1)),
        RefreshTokenFactory.Create(
            new Guid("90000000-9000-0000-0000-900000000123"),
            Users.First())
    ];
}