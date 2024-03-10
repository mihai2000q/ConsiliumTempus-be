using ConsiliumTempus.Api.IntegrationTests.Core;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.TestCollections;

[CollectionDefinition(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerCollection : ICollectionFixture<WebAppFactory>;