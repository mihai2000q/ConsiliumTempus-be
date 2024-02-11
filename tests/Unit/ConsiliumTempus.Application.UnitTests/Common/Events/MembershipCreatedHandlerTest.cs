using ConsiliumTempus.Application.Common.Events;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Events;

namespace ConsiliumTempus.Application.UnitTests.Common.Events;

public class MembershipCreatedHandlerTest
{
    #region Setup

    private readonly Mock<IWorkspaceRoleRepository> _workspaceRoleRepository;
    private readonly MembershipCreatedHandler _uut;
    
    public MembershipCreatedHandlerTest()
    {
        _workspaceRoleRepository = new Mock<IWorkspaceRoleRepository>();
        _uut = new MembershipCreatedHandler(_workspaceRoleRepository.Object);
    }

    #endregion

    [Fact]
    public async Task WhenMembershipIsCreated_ShouldAttachTheWorkspaceRole()
    {
        // Arrange
        var membership = Mock.Mock.Membership.CreateMock();
        var domainEvent = new MembershipCreated(membership);
        
        // Act
        await _uut.Handle(domainEvent, default);

        // Assert
        _workspaceRoleRepository.Verify(w => w.Attach(membership.WorkspaceRole), 
            Times.Once());
    }
}