namespace ConsiliumTempus.Application.UnitTests.TestData.Common.Extensions;

internal static partial class Data
{
    internal static class StringExtensions
    {
        internal class GetValidTruncateAggregate : TheoryData<string, string>
        {
            public GetValidTruncateAggregate()
            {
                Add("WorkspaceAggregate", "Workspace");
                Add("WorkspaceAggreg", "WorkspaceAggreg");
                Add("Workspace", "Workspace");
            }
        }
        
        internal class GetValidEmails : TheoryData<string>
        {
            public GetValidEmails()
            {
                Add("Some@Example.com");
                Add("Marcel@yahoo.com");
            }
        }
        
        internal class GetInvalidEmails : TheoryData<string>
        {
            public GetInvalidEmails()
            {
                Add("SomeExample");
                Add("Some@Example");
                Add("SomeExample.com");
            }
        }
        
        internal class GetCapitalizeStrings : TheoryData<string, string>
        {
            public GetCapitalizeStrings()
            {
                Add("aNdReI", "Andrei");
                Add("mIchAel JaMES", "Michael James");
                Add("CHRISTIAN-MICHAEL", "Christian-Michael");
            }
        }
    }
}