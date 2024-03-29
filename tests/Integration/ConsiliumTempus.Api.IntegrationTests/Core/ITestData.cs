namespace ConsiliumTempus.Api.IntegrationTests.Core;

public interface ITestData
{
    internal IEnumerable<object[]> GetDataCollections();
}