using ConsiliumTempus.Api.IntegrationTests.Core;

namespace ConsiliumTempus.Api.IntegrationTests.TestCollections;

[CollectionDefinition(nameof(ProjectStageControllerCollection))]
public class ProjectStageControllerCollection : ICollectionFixture<WebAppFactory>;