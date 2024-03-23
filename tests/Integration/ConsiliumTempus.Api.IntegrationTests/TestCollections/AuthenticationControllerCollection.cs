using ConsiliumTempus.Api.IntegrationTests.Core;

namespace ConsiliumTempus.Api.IntegrationTests.TestCollections;

[CollectionDefinition(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerCollection : ICollectionFixture<WebAppFactory>;