using ConsiliumTempus.Application.Common.Events;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Domain.Common.Events;

namespace ConsiliumTempus.Application.UnitTests.Common.Events;

public class MembershipCreatedHandlerTest
{
    #region Setup

    private readonly IWorkspaceRoleRepository _workspaceRoleRepository;
    private readonly MembershipCreatedHandler _uut;

    public MembershipCreatedHandlerTest()
    {
        _workspaceRoleRepository = Substitute.For<IWorkspaceRoleRepository>();
        _uut = new MembershipCreatedHandler(_workspaceRoleRepository);
    }

    #endregion

    [Fact]
    public async Task WhenMembershipIsCreated_ShouldAttachTheWorkspaceRole()
    {
        // Arrange
        var membership = MembershipFactory.Create();
        var domainEvent = new MembershipCreated(membership);

        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        _workspaceRoleRepository
            .Received(1)
            .Attach(membership.WorkspaceRole);
    }
}