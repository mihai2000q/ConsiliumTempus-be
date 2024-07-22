using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Workspace.Queries.GetInvitations;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetInvitations;

public static class GetInvitationsWorkspaceQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetInvitationsWorkspaceQuery>
    {
        public GetValidQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetInvitationsWorkspaceQuery(isSender: true);
            Add(query);

            query = new GetInvitationsWorkspaceQuery(
                null,
                Guid.NewGuid(),
                10,
                1);
            Add(query);
        }
    }

    internal class GetInvalidIsSenderAndWorkspaceIdQueries : TheoryData<GetInvitationsWorkspaceQuery, string>
    {
        public GetInvalidIsSenderAndWorkspaceIdQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetInvitationsWorkspaceQuery();
            Add(query, nameof(query.IsSender).And(nameof(query.WorkspaceId)));
            
            query = WorkspaceQueryFactory.CreateGetInvitationsWorkspaceQuery(
                isSender: true,
                workspaceId: Guid.NewGuid());
            Add(query, nameof(query.IsSender).And(nameof(query.WorkspaceId)));
        }
    }

    internal class GetInvalidPageSizeAndCurrentPageQueries : TheoryData<GetInvitationsWorkspaceQuery, string>
    {
        public GetInvalidPageSizeAndCurrentPageQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetInvitationsWorkspaceQuery(
                isSender: true,
                pageSize: 1);
            Add(query, nameof(query.PageSize).And(nameof(query.CurrentPage)));

            query = WorkspaceQueryFactory.CreateGetInvitationsWorkspaceQuery(
                isSender: true,
                currentPage: 1);
            Add(query, nameof(query.PageSize).And(nameof(query.CurrentPage)));
        }
    }

    internal class GetInvalidPageSizeQueries : TheoryData<GetInvitationsWorkspaceQuery, string>
    {
        public GetInvalidPageSizeQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetInvitationsWorkspaceQuery(
                isSender: true,
                currentPage: 1,
                pageSize: -1);
            Add(query, nameof(query.PageSize));

            query = WorkspaceQueryFactory.CreateGetInvitationsWorkspaceQuery(
                isSender: true,
                currentPage: 1,
                pageSize: 0);
            Add(query, nameof(query.PageSize));
        }
    }

    internal class GetInvalidCurrentPageQueries : TheoryData<GetInvitationsWorkspaceQuery, string>
    {
        public GetInvalidCurrentPageQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetInvitationsWorkspaceQuery(
                isSender: true,
                pageSize: 20,
                currentPage: -1);
            Add(query, nameof(query.CurrentPage));

            query = WorkspaceQueryFactory.CreateGetInvitationsWorkspaceQuery(
                isSender: true,
                pageSize: 20,
                currentPage: 0);
            Add(query, nameof(query.CurrentPage));
        }
    }
}