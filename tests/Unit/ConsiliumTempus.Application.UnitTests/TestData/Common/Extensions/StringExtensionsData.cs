namespace ConsiliumTempus.Application.UnitTests.TestData.Common.Extensions;

internal static class StringExtensionsData
{
    internal class GetValidToId : TheoryData<string, string>
    {
        public GetValidToId()
        {
            Add("", "Id");
            Add("role", "roleId");
            Add("BigRole", "BigRoleId");
        }
    }

    internal class GetValidToBackingField : TheoryData<string, string>
    {
        public GetValidToBackingField()
        {
            Add("", "_");
            Add("a", "_a");
            Add("ab", "_ab");
            Add("aB", "_aB");
            Add("Role", "_role");
            Add("WorkspaceRole", "_workspaceRole");
        }
    }

    internal class GetValidToIdBackingField : TheoryData<string, string>
    {
        public GetValidToIdBackingField()
        {
            Add("", "_Id");
            Add("a", "_aId");
            Add("ab", "_abId");
            Add("aB", "_aBId");
            Add("Role", "_roleId");
            Add("WorkspaceRole", "_workspaceRoleId");
        }
    }

    internal class GetValidTruncateAggregate : TheoryData<string, string>
    {
        public GetValidTruncateAggregate()
        {
            Add("WorkspaceAggregate", "Workspace");
            Add("WorkspaceAggreg", "WorkspaceAggreg");
            Add("Workspace", "Workspace");
            Add("", "");
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