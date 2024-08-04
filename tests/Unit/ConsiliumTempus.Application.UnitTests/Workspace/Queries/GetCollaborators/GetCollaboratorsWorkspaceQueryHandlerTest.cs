using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Filters;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.GetCollaborators;

public class GetCollaboratorsFromWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly GetCollaboratorsFromWorkspaceQueryHandler _uut;

    public GetCollaboratorsFromWorkspaceQueryHandlerTest()
    {
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new GetCollaboratorsFromWorkspaceQueryHandler(_workspaceRepository);
    }

    #endregion

    [Fact]
    public async Task HandleGetCollaboratorsFromWorkspaceQuery_WhenIsSuccessful_ShouldReturnCollaborators()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetCollaboratorsFromWorkspaceQuery();

        var collaborators = MembershipFactory.CreateList();
        _workspaceRepository
            .GetCollaborators(Arg.Any<WorkspaceId>(), 
                Arg.Any<string>(),
                Arg.Any<IReadOnlyList<IFilter<Membership>>>(),
                Arg.Any<IReadOnlyList<IOrder<Membership>>>(),
                Arg.Any<PaginationInfo?>())
            .Returns(collaborators);

        const int totalCount = 25;
        _workspaceRepository
            .GetCollaboratorsCount(Arg.Any<WorkspaceId>(), 
                Arg.Any<string>(),
                Arg.Any<IReadOnlyList<IFilter<Membership>>>())
            .Returns(totalCount);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _workspaceRepository
            .Received(1)
            .GetCollaborators(
                Arg.Is<WorkspaceId>(id => query.Id == id.Value),
                Arg.Is<string>(s => s == query.SearchValue),
                Arg.Is<IReadOnlyList<IFilter<Membership>>>(filters => 
                    filters.AssertFilters(query.Search, MembershipFilter.FilterProperties)),
                Arg.Is<IReadOnlyList<IOrder<Membership>>>(orders => 
                    orders.AssertOrders(query.OrderBy, MembershipOrder.OrderProperties)),
                Arg.Is<PaginationInfo?>(p => 
                    p.AssertPagination(query.PageSize, query.CurrentPage)));
        await _workspaceRepository
            .Received(1)
            .GetCollaboratorsCount(
                Arg.Is<WorkspaceId>(id => query.Id == id.Value),
                Arg.Is<string>(s => s == query.SearchValue),
                Arg.Is<IReadOnlyList<IFilter<Membership>>>(filters => 
                    filters.AssertFilters(query.Search, MembershipFilter.FilterProperties)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Collaborators.Should().BeEquivalentTo(collaborators);
    }
}