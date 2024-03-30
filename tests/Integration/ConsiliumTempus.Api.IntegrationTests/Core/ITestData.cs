namespace ConsiliumTempus.Api.IntegrationTests.Core;

public interface ITestData
{
    IEnumerable<IEnumerable<object>> GetDataCollections();
}