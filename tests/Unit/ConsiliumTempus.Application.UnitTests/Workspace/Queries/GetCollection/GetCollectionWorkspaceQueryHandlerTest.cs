using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetCollection;
using ConsiliumTempus.Application.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.Orders;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.Workspace.Queries.GetCollection;

public class GetCollectionWorkspaceQueryHandlerTest
{
    #region Setup

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly GetCollectionWorkspaceQueryHandler _uut;

    public GetCollectionWorkspaceQueryHandlerTest()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _workspaceRepository = Substitute.For<IWorkspaceRepository>();
        _uut = new GetCollectionWorkspaceQueryHandler(_currentUserProvider, _workspaceRepository);
    }

    #endregion

    [Theory]
    [ClassData(typeof(GetCollectionWorkspaceQueryHandlerData.GetQueries))]
    public async Task WhenGetCollectionWorkspaceIsSuccessful_ShouldReturnCollectionOfWorkspaces(
        GetCollectionWorkspaceQuery query)
    {
        // Arrange
        var user = UserFactory.Create();
        _currentUserProvider
            .GetCurrentUser()
            .Returns(user);

        var workspaces = WorkspaceFactory.CreateList();
        _workspaceRepository
            .GetListByUser(
                Arg.Any<UserAggregate>(), 
                Arg.Any<PaginationInfo?>(),
                Arg.Any<IOrder<WorkspaceAggregate>?>(),
                Arg.Any<IEnumerable<IFilter<WorkspaceAggregate>>>())
            .Returns(workspaces);

        const int workspacesCount = 25;
        _workspaceRepository
            .GetListByUserCount(
                Arg.Any<UserAggregate>(), 
                Arg.Any<IEnumerable<IFilter<WorkspaceAggregate>>>())
            .Returns(workspacesCount);

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();
        await _workspaceRepository
            .Received(1)
            .GetListByUser(
                Arg.Is<UserAggregate>(u => u == user),
                Arg.Is<PaginationInfo?>(p => p.AssertPagination(query.PageSize, query.CurrentPage)),
                Arg.Is<IOrder<WorkspaceAggregate>?>(o => 
                    o.AssertOrder(query.Order, WorkspaceOrder.OrderProperties)),
                Arg.Is<IEnumerable<IFilter<WorkspaceAggregate>>>(filters => 
                    Utils.Workspace.AssertGetCollectionFilters(filters, query)));
        
        await _workspaceRepository
            .Received(1)
            .GetListByUserCount(
                Arg.Is<UserAggregate>(u => u == user),
                Arg.Is<IEnumerable<IFilter<WorkspaceAggregate>>>(filters => 
                    Utils.Workspace.AssertGetCollectionFilters(filters, query)));

        outcome.IsError.Should().BeFalse();
        outcome.Value.Workspaces.Should().BeEquivalentTo(workspaces);
        outcome.Value.TotalCount.Should().Be(workspacesCount);
        if (query.PageSize is null || query.CurrentPage is null)
            outcome.Value.TotalPages.Should().BeNull();
        else
            outcome.Value.TotalPages.Should().Be(workspacesCount / query.PageSize);
    }
    
    [Fact]
    public async Task WhenGetCollectionWorkspaceFails_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery();

        // Act
        var outcome = await _uut.Handle(query, default);

        // Assert
        await _currentUserProvider
            .Received(1)
            .GetCurrentUser();
        _workspaceRepository.DidNotReceive();
        
        outcome.ValidateError(Errors.User.NotFound);
    }
}