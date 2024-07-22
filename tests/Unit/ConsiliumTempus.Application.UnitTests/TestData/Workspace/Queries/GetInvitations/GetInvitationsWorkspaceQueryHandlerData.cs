using ConsiliumTempus.Application.Workspace.Queries.GetInvitations;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetInvitations;

internal static class GetInvitationsWorkspaceQueryHandlerData
{
    internal class GetQueries : TheoryData<GetInvitationsWorkspaceQuery>
    {
        public GetQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetInvitationsWorkspaceQuery(
                isSender: true);
            Add(query);

            query = WorkspaceQueryFactory.CreateGetInvitationsWorkspaceQuery(
                workspaceId: Guid.NewGuid());
            Add(query);
        }
    }
}