using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetCollaborators;

public static class GetCollaboratorsFromWorkspaceQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCollaboratorsFromWorkspaceQuery>
    {
        public GetValidQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery();
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy: [],
                search: []);
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy:
                [
                    "user_email.asc", "user_first_name.asc", "user_last_name.asc", "user_name.asc",
                    "workspace_role_id.asc", "workspace_role_name.asc",
                    "created_date_time.asc", "updated_date_time.asc"
                ]);
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy:
                [
                    "user_email.desc", "user_first_name.desc", "user_last_name.desc", "user_name.desc",
                    "workspace_role_id.desc", "workspace_role_name.desc",
                    "created_date_time.desc", "updated_date_time.desc"
                ]);
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search:
                [
                    // User Name
                    "user_name ct something", "user_name sw something", "user_name eq something",
                    "user_name neq something",
                    // Workspace Role ID
                    "workspace_role_id eq 1", "workspace_role_id neq 2",
                    "workspace_role_id lt 1", "workspace_role_id lte 2",
                    "workspace_role_id gt 1", "workspace_role_id gte 2",
                    // Workspace Role Name
                    "workspace_role_name ct something", "workspace_role_name sw something",
                    "workspace_role_name eq something", "workspace_role_name neq something",
                ]);
            Add(query);

            query = new GetCollaboratorsFromWorkspaceQuery(
                Guid.NewGuid(),
                1,
                10,
                ["workspace_role_id.desc", "user_email.asc"],
                ["user_name sw Cla"],
                null);
            Add(query);
        }
    }

    internal class GetInvalidIdQueries : TheoryData<GetCollaboratorsFromWorkspaceQuery, string, short>
    {
        public GetInvalidIdQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(id: Guid.Empty);
            Add(query, nameof(query.Id), 1);
        }
    }

    internal class
        GetInvalidPageSizeAndCurrentPageQueries : TheoryData<GetCollaboratorsFromWorkspaceQuery, string, short>
    {
        public GetInvalidPageSizeAndCurrentPageQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                pageSize: 1);
            Add(query, nameof(query.PageSize).And(nameof(query.CurrentPage)), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                currentPage: 1);
            Add(query, nameof(query.PageSize).And(nameof(query.CurrentPage)), 1);
        }
    }

    internal class GetInvalidPageSizeQueries : TheoryData<GetCollaboratorsFromWorkspaceQuery, string, short>
    {
        public GetInvalidPageSizeQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                currentPage: 1,
                pageSize: -1);
            Add(query, nameof(query.PageSize), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                currentPage: 1,
                pageSize: 0);
            Add(query, nameof(query.PageSize), 1);
        }
    }

    internal class GetInvalidCurrentPageQueries : TheoryData<GetCollaboratorsFromWorkspaceQuery, string, short>
    {
        public GetInvalidCurrentPageQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                pageSize: 20,
                currentPage: -1);
            Add(query, nameof(query.CurrentPage), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                pageSize: 20,
                currentPage: 0);
            Add(query, nameof(query.CurrentPage), 1);
        }
    }

    internal class GetInvalidOrderByQueries : TheoryData<GetCollaboratorsFromWorkspaceQuery, string, short>
    {
        public GetInvalidOrderByQueries()
        {
            const string correct = "user_name.asc";

            // Separator Validation
            var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy: [""]);
            Add(query, nameof(query.OrderBy), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy: ["something"]);
            Add(query, nameof(query.OrderBy), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy: [correct, "something", "another"]);
            Add(query, nameof(query.OrderBy), 1);

            // Order Type Validation
            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy: ["user_name."]);
            Add(query, nameof(query.OrderBy), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy: ["user_name.descending"]);
            Add(query, nameof(query.OrderBy), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy: [correct, "workspace_role_id.descending"]);
            Add(query, nameof(query.OrderBy), 1);

            // Snake Case Validation
            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy: ["WorkspaceRoleId.desc"]);
            Add(query, nameof(query.OrderBy), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy: [correct, "WorkspaceRoleId.desc"]);
            Add(query, nameof(query.OrderBy), 1);

            // Property Validation
            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy: ["not_a_property.asc"]);
            Add(query, nameof(query.OrderBy), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy: [correct, "not_property.desc"]);
            Add(query, nameof(query.OrderBy), 1);

            // Repetition Validation
            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                orderBy: ["workspace_role_id.asc", "workspace_role_id.desc"]);
            Add(query, nameof(query.OrderBy), 1);
        }
    }

    internal class GetInvalidSearchQueries : TheoryData<GetCollaboratorsFromWorkspaceQuery, string, short>
    {
        public GetInvalidSearchQueries()
        {
            const string correct = "user_name eq something";

            // Separator Validation
            var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: [""]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: ["user_name"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: ["user_name eq"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: ["user_name eq "]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: ["user_name eq     "]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: [correct, "user_name eq     "]);
            Add(query, nameof(query.Search), 1);

            // Snake Case Validation
            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: ["UserName eq something"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: [correct, "UserName eq something"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: ["NotProperty eq something"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: [correct, "NotProperty eq something"]);
            Add(query, nameof(query.Search), 1);

            // Property Validation
            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: ["not_a_property eq something"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: [correct, "not_a_property eq something"]);
            Add(query, nameof(query.Search), 1);

            // Operator Validation
            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: ["user_name smth something"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: [correct, "user_name smth something"]);
            Add(query, nameof(query.Search), 1);

            // Operator And Type Validation
            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: ["user_name lte something"]);
            Add(query, nameof(query.Search), 1);

            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: [correct, "workspace_role_id ct 1"]);
            Add(query, nameof(query.Search), 1);

            // Parse Validation
            query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery(
                search: ["workspace_role_id eq something"]);
            Add(query, nameof(query.Search), 1);
        }
    }
}