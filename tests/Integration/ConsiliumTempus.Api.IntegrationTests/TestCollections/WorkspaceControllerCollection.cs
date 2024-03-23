using ConsiliumTempus.Api.IntegrationTests.Core;

namespace ConsiliumTempus.Api.IntegrationTests.TestCollections;

[CollectionDefinition(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerCollection : ICollectionFixture<WebAppFactory>;