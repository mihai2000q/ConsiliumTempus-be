using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetCollaborators;

internal static class GetCollaboratorsFromWorkspaceQueryHandlerData
{
    internal class GetQueries : TheoryData<GetCollaboratorsFromWorkspaceQuery>
    {
        public GetQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery();
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                pageSize: 12,
                currentPage: 1,
                search: ["user_name ct John"],
                searchValue: "john");
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: ["user_name sw John", "workspace_role_id eq 1", "workspace_role_name neq Viewer"]);
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy:
                [
                    "user_email.asc",
                    "user_first_name.desc",
                    "user_last_name.asc",
                    "user_name.desc",
                    "workspace_role_id.asc",
                    "workspace_role_name.desc",
                    "created_date_time.asc",
                    "updated_date_time.desc"
                ]);
            Add(query);
        }
    }
}
