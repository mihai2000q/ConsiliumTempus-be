using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetInvitations;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Queries.GetInvitations;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Common.UnitTests.Workspace.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using NSubstitute.ReturnsExtensions;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.GetInvitations;

public class GetInvitationsWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly GetInvitationsWorkspaceQueryHandler _uut;

    public GetInvitationsWorkspaceQueryHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new GetInvitationsWorkspaceQueryHandler(_currentUserProvider, _workspaceRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(GetInvitationsWorkspaceQueryHandlerData.GetQueries))]
    public async Task HandleGetInvitationsWorkspaceQuery_WhenIsSuccessful_ShouldReturnInvitations(
        GetInvitationsWorkspaceQuery query)
    {
        // Arrange
        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUser()
            .Returns(user);

        var invitations = WorkspaceInvitationFactory.CreateList();
        _workspaceRepository
            .GetInvitations(
                Arg.Any<UserAggregate?>(),
                Arg.Any<bool?>(),
                Arg.Any<WorkspaceId?>(),
                Arg.Any<PaginationInfo?>())
            .Returns(invitations);

        const int totalCount = 25;
        _workspaceRepository
            .GetInvitationsCount(
                Arg.Any<UserAggregate?>(),
                Arg.Any<bool?>(),
                Arg.Any<WorkspaceId?>())
            .Returns(totalCount);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        if (query.IsSender is not null)
            await _currentUserProvider
                .Received(1)
                .GetCurrentUser();

        await _workspaceRepository
            .Received(1)
            .GetInvitations(
                Arg.Is<UserAggregate?>(u => query.IsSender != null ? u == user : u == null),
                Arg.Is(query.IsSender),
                Arg.Is<WorkspaceId?>(wId => query.WorkspaceId != null ? wId!.Value == query.WorkspaceId : wId == null),
                Arg.Is<PaginationInfo?>(paginationInfo =>
                    paginationInfo.AssertPagination(query.PageSize, query.CurrentPage)));

        await _workspaceRepository
            .Received(1)
            .GetInvitationsCount(
                Arg.Is<UserAggregate?>(u => query.IsSender != null ? u == user : u == null),
                Arg.Is(query.IsSender),
                Arg.Is<WorkspaceId?>(wId => query.WorkspaceId != null ? wId!.Value == query.WorkspaceId : wId == null));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Invitations.Should().BeEquivalentTo(invitations);
        outcome.Value.TotalCount.Should().Be(totalCount);
    }

    [Fact]
    public async Task HandleGetInvitationsWorkspaceQuery_WhenUserIsNull_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetInvitationsWorkspaceQuery(isSender: false);

        _currentUserProvider
            .GetCurrentUser()
            .ReturnsNull();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();

        outcome.ValidateError(Errors.User.NotFound);
    }
}