using ConsiliumTempus.Api.IntegrationTests.Core;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.TestCollections;

[CollectionDefinition(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerCollection : ICollectionFixture<WebAppFactory>;