using ConsiliumTempus.Api.IntegrationTests.Core;

namespace ConsiliumTempus.Api.IntegrationTests.TestCollections;

[CollectionDefinition(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerCollection : ICollectionFixture<WebAppFactory>;