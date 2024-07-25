using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Workspace.AcceptInvitation;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.GetInvitations;
using ConsiliumTempus.Api.Contracts.Workspace.GetOverview;
using ConsiliumTempus.Api.Contracts.Workspace.InviteCollaborator;
using ConsiliumTempus.Api.Contracts.Workspace.Leave;
using ConsiliumTempus.Api.Contracts.Workspace.RejectInvitation;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateFavorites;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateOverview;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;
using ConsiliumTempus.Application.Workspace.Commands.Leave;
using ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Application.Workspace.Queries.GetInvitations;
using ConsiliumTempus.Application.Workspace.Queries.GetOverview;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class WorkspaceControllerTest
{
    #region Setup

    private readonly ISender _mediator;
    private readonly WorkspaceController _uut;

    public WorkspaceControllerTest()
    {
        var mapper = Utils.GetMapper<WorkspaceMappingConfig>();

        _mediator = Substitute.For<ISender>();
        _uut = new WorkspaceController(mapper, _mediator);

        Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task Get_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateGetWorkspaceResult();
        _mediator
            .Send(Arg.Any<GetWorkspaceQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetWorkspaceQuery>(query => Utils.Workspace.AssertGetQuery(query, request)));

        var response = outcome.ToResponse<GetWorkspaceResponse>();
        Utils.Workspace.AssertGetResponse(response, result);
    }

    [Fact]
    public async Task Get_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<GetWorkspaceQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetWorkspaceQuery>(query => Utils.Workspace.AssertGetQuery(query, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetOverview_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetOverviewWorkspaceRequest();

        var workspace = WorkspaceFactory.Create();
        _mediator
            .Send(Arg.Any<GetOverviewWorkspaceQuery>())
            .Returns(workspace);

        // Act
        var outcome = await _uut.GetOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetOverviewWorkspaceQuery>(query =>
                Utils.Workspace.AssertGetOverviewQuery(query, request)));

        var response = outcome.ToResponse<GetOverviewWorkspaceResponse>();
        Utils.Workspace.AssertGetOverviewResponse(response, workspace);
    }

    [Fact]
    public async Task GetOverview_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetOverviewWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<GetOverviewWorkspaceQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetOverviewWorkspaceQuery>(query =>
                Utils.Workspace.AssertGetOverviewQuery(query, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetCollection_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetCollectionWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateGetCollectionWorkspaceResult();
        _mediator
            .Send(Arg.Any<GetCollectionWorkspaceQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollection(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionWorkspaceQuery>(query =>
                Utils.Workspace.AssertGetCollectionQuery(query, request)));

        var response = outcome.ToResponse<GetCollectionWorkspaceResponse>();
        Utils.Workspace.AssertGetCollectionResponse(response, result);
    }

    [Fact]
    public async Task GetCollection_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetCollectionWorkspaceRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<GetCollectionWorkspaceQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollection(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionWorkspaceQuery>(query =>
                Utils.Workspace.AssertGetCollectionQuery(query, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetCollaborators_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetCollaboratorsFromWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateGetCollaboratorsFromWorkspaceResult();
        _mediator
            .Send(Arg.Any<GetCollaboratorsFromWorkspaceQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollaborators(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollaboratorsFromWorkspaceQuery>(query =>
                Utils.Workspace.AssertGetCollaboratorsQuery(query, request)));

        var response = outcome.ToResponse<GetCollaboratorsFromWorkspaceResponse>();
        Utils.Workspace.AssertGetCollaboratorsResponse(response, result);
    }

    [Fact]
    public async Task GetCollaborators_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetCollaboratorsFromWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<GetCollaboratorsFromWorkspaceQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollaborators(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollaboratorsFromWorkspaceQuery>(query =>
                Utils.Workspace.AssertGetCollaboratorsQuery(query, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetInvitations_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetInvitationsWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateGetInvitationsWorkspaceResult();
        _mediator
            .Send(Arg.Any<GetInvitationsWorkspaceQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetInvitations(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetInvitationsWorkspaceQuery>(query =>
                Utils.Workspace.AssertGetInvitationsQuery(query, request)));

        var response = outcome.ToResponse<GetInvitationsWorkspaceResponse>();
        Utils.Workspace.AssertGetInvitationsResponse(response, result);
    }

    [Fact]
    public async Task GetInvitations_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetInvitationsWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<GetInvitationsWorkspaceQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetInvitations(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetInvitationsWorkspaceQuery>(query =>
                Utils.Workspace.AssertGetInvitationsQuery(query, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Create_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateCreateWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateCreateWorkspaceResult();
        _mediator
            .Send(Arg.Any<CreateWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateWorkspaceCommand>(
                command => Utils.Workspace.AssertCreateCommand(command, request)));

        var response = outcome.ToResponse<CreateWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Create_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateCreateWorkspaceRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<CreateWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateWorkspaceCommand>(
                command => Utils.Workspace.AssertCreateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task InviteCollaborator_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateInviteCollaboratorToWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateInviteCollaboratorToWorkspaceResult();
        _mediator
            .Send(Arg.Any<InviteCollaboratorToWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.InviteCollaborator(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<InviteCollaboratorToWorkspaceCommand>(
                command => Utils.Workspace.AssertInviteCollaboratorCommand(command, request)));

        var response = outcome.ToResponse<InviteCollaboratorToWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task InviteCollaborator_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateInviteCollaboratorToWorkspaceRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<InviteCollaboratorToWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.InviteCollaborator(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<InviteCollaboratorToWorkspaceCommand>(
                command => Utils.Workspace.AssertInviteCollaboratorCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task AcceptInvitation_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateAcceptInvitationToWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateAcceptInvitationToWorkspaceResult();
        _mediator
            .Send(Arg.Any<AcceptInvitationToWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.AcceptInvitation(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<AcceptInvitationToWorkspaceCommand>(
                command => Utils.Workspace.AssertAcceptInvitationCommand(command, request)));

        var response = outcome.ToResponse<AcceptInvitationToWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task AcceptInvitation_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateAcceptInvitationToWorkspaceRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<AcceptInvitationToWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.AcceptInvitation(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<AcceptInvitationToWorkspaceCommand>(
                command => Utils.Workspace.AssertAcceptInvitationCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task RejectInvitation_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateRejectInvitationToWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateRejectInvitationToWorkspaceResult();
        _mediator
            .Send(Arg.Any<RejectInvitationToWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.RejectInvitation(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RejectInvitationToWorkspaceCommand>(
                command => Utils.Workspace.AssertRejectInvitationCommand(command, request)));

        var response = outcome.ToResponse<RejectInvitationToWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task RejectInvitation_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateRejectInvitationToWorkspaceRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<RejectInvitationToWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.RejectInvitation(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RejectInvitationToWorkspaceCommand>(
                command => Utils.Workspace.AssertRejectInvitationCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Leave_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateLeaveWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateLeaveWorkspaceResult();
        _mediator
            .Send(Arg.Any<LeaveWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Leave(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<LeaveWorkspaceCommand>(
                command => Utils.Workspace.AssertLeaveCommand(command, request)));

        var response = outcome.ToResponse<LeaveWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Leave_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateLeaveWorkspaceRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<LeaveWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Leave(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<LeaveWorkspaceCommand>(
                command => Utils.Workspace.AssertLeaveCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Update_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateUpdateWorkspaceResult();
        _mediator
            .Send(Arg.Any<UpdateWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateWorkspaceCommand>(
                command => Utils.Workspace.AssertUpdateCommand(command, request)));

        var response = outcome.ToResponse<UpdateWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Update_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<UpdateWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateWorkspaceCommand>(
                command => Utils.Workspace.AssertUpdateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task UpdateFavorites_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateFavoritesWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateUpdateFavoritesWorkspaceResult();
        _mediator
            .Send(Arg.Any<UpdateFavoritesWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.UpdateFavorites(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateFavoritesWorkspaceCommand>(
                command => Utils.Workspace.AssertUpdateFavoritesCommand(command, request)));

        var response = outcome.ToResponse<UpdateFavoritesWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateFavorites_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateFavoritesWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<UpdateFavoritesWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.UpdateFavorites(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateFavoritesWorkspaceCommand>(
                command => Utils.Workspace.AssertUpdateFavoritesCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task UpdateOverview_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateOverviewWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateUpdateOverviewWorkspaceResult();
        _mediator
            .Send(Arg.Any<UpdateOverviewWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.UpdateOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateOverviewWorkspaceCommand>(
                command => Utils.Workspace.AssertUpdateOverviewCommand(command, request)));

        var response = outcome.ToResponse<UpdateOverviewWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateOverview_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateOverviewWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<UpdateOverviewWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.UpdateOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateOverviewWorkspaceCommand>(
                command => Utils.Workspace.AssertUpdateOverviewCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Delete_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateDeleteWorkspaceRequest();

        var result = WorkspaceResultFactory.CreateDeleteWorkspaceResult();
        _mediator
            .Send(Arg.Any<DeleteWorkspaceCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteWorkspaceCommand>(command =>
                Utils.Workspace.AssertDeleteCommand(command, request)));

        var response = outcome.ToResponse<DeleteWorkspaceResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Delete_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateDeleteWorkspaceRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<DeleteWorkspaceCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteWorkspaceCommand>(command =>
                Utils.Workspace.AssertDeleteCommand(command, request)));

        outcome.ValidateError(error);
    }
}