using ConsiliumTempus.Api.IntegrationTests.Core;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.TestCollections;

[CollectionDefinition(nameof(ProjectControllerCollection))]
public class ProjectControllerCollection : ICollectionFixture<WebAppFactory>;