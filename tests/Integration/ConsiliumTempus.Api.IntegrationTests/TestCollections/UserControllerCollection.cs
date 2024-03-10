using ConsiliumTempus.Api.IntegrationTests.Core;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.TestCollections;

[CollectionDefinition(nameof(UserControllerCollection))]
public class UserControllerCollection : ICollectionFixture<WebAppFactory>;