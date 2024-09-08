using ConsiliumTempus.Api.IntegrationTests.Core;

namespace ConsiliumTempus.Api.IntegrationTests.TestCollections;

[CollectionDefinition(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerCollection : ICollectionFixture<WebAppFactory>;