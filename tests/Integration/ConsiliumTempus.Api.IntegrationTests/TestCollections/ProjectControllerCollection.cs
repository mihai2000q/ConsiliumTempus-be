using ConsiliumTempus.Api.IntegrationTests.Core;

namespace ConsiliumTempus.Api.IntegrationTests.TestCollections;

[CollectionDefinition(nameof(ProjectControllerCollection))]
public class ProjectControllerCollection : ICollectionFixture<WebAppFactory>;