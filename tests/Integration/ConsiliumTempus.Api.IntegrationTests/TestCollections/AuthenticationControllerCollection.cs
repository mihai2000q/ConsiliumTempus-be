using ConsiliumTempus.Api.IntegrationTests.Core;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.TestCollections;

[CollectionDefinition(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerCollection : ICollectionFixture<WebAppFactory>;