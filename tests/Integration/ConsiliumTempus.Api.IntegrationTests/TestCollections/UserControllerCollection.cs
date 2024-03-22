using ConsiliumTempus.Api.IntegrationTests.Core;

namespace ConsiliumTempus.Api.IntegrationTests.TestCollections;

[CollectionDefinition(nameof(UserControllerCollection))]
public class UserControllerCollection : ICollectionFixture<WebAppFactory>;