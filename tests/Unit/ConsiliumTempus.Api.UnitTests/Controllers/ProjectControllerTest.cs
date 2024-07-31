using ConsiliumTempus.Api.Common.Mapping;
using ConsiliumTempus.Api.Contracts.Project.AddAllowedMember;
using ConsiliumTempus.Api.Contracts.Project.AddStatus;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetAllowedMembers;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetOverview;
using ConsiliumTempus.Api.Contracts.Project.GetStatuses;
using ConsiliumTempus.Api.Contracts.Project.LeavePrivate;
using ConsiliumTempus.Api.Contracts.Project.RemoveAllowedMember;
using ConsiliumTempus.Api.Contracts.Project.RemoveStatus;
using ConsiliumTempus.Api.Contracts.Project.Update;
using ConsiliumTempus.Api.Contracts.Project.UpdateFavorites;
using ConsiliumTempus.Api.Contracts.Project.UpdateOverview;
using ConsiliumTempus.Api.Contracts.Project.UpdateOwner;
using ConsiliumTempus.Api.Contracts.Project.UpdatePrivacy;
using ConsiliumTempus.Api.Contracts.Project.UpdateStatus;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Api.UnitTests.TestUtils;
using ConsiliumTempus.Application.Project.Commands.AddAllowedMember;
using ConsiliumTempus.Application.Project.Commands.AddStatus;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.LeavePrivate;
using ConsiliumTempus.Application.Project.Commands.RemoveAllowedMember;
using ConsiliumTempus.Application.Project.Commands.RemoveStatus;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.Project.Commands.UpdateOwner;
using ConsiliumTempus.Application.Project.Commands.UpdatePrivacy;
using ConsiliumTempus.Application.Project.Commands.UpdateStatus;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetAllowedMembers;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Application.Project.Queries.GetStatuses;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;

namespace ConsiliumTempus.Api.UnitTests.Controllers;

public class ProjectControllerTest
{
    #region Setup

    private readonly ISender _mediator;
    private readonly ProjectController _uut;

    public ProjectControllerTest()
    {
        var mapper = Utils.GetMapper<ProjectMappingConfig>();

        _mediator = Substitute.For<ISender>();
        _uut = new ProjectController(mapper, _mediator);

        Utils.ResolveHttpContext(_uut);
    }

    #endregion

    [Fact]
    public async Task Get_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetProjectRequest();

        var result = ProjectResultFactory.CreateGetProjectResult();
        _mediator
            .Send(Arg.Any<GetProjectQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetProjectQuery>(q =>
                Utils.Project.AssertGetProjectQuery(q, request)));

        var response = outcome.ToResponse<GetProjectResponse>();
        Utils.Project.AssertGetProjectResponse(response, result);
    }

    [Fact]
    public async Task Get_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<GetProjectQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.Get(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetProjectQuery>(q =>
                Utils.Project.AssertGetProjectQuery(q, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetOverview_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetOverviewProjectRequest();

        var projectOverview = new GetOverviewProjectResult(Description.Create("this is a description"));
        _mediator
            .Send(Arg.Any<GetOverviewProjectQuery>())
            .Returns(projectOverview);

        // Act
        var outcome = await _uut.GetOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetOverviewProjectQuery>(q =>
                Utils.Project.AssertGetOverviewProjectQuery(q, request)));

        var response = outcome.ToResponse<GetOverviewProjectResponse>();
        Utils.Project.AssertGetOverviewProjectResponse(response, projectOverview);
    }

    [Fact]
    public async Task GetOverview_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetOverviewProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<GetOverviewProjectQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetOverviewProjectQuery>(q =>
                Utils.Project.AssertGetOverviewProjectQuery(q, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetCollection_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest();

        var result = ProjectResultFactory.CreateGetCollectionProjectResult();
        _mediator
            .Send(Arg.Any<GetCollectionProjectQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetCollection(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionProjectQuery>(q =>
                Utils.Project.AssertGetCollectionProjectQuery(q, request)));

        var response = outcome.ToResponse<GetCollectionProjectResponse>();
        Utils.Project.AssertGetCollectionResponse(response, result);
    }

    [Fact]
    public async Task GetCollection_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest();

        var error = Errors.User.NotFound;
        _mediator
            .Send(Arg.Any<GetCollectionProjectQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetCollection(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetCollectionProjectQuery>(q =>
                Utils.Project.AssertGetCollectionProjectQuery(q, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetStatuses_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetStatusesFromProjectRequest();

        var result = ProjectResultFactory.CreateGetStatusesFromProjectResult();
        _mediator
            .Send(Arg.Any<GetStatusesFromProjectQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetStatuses(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetStatusesFromProjectQuery>(q =>
                Utils.Project.AssertGetStatusesFromProjectQuery(q, request)));

        var response = outcome.ToResponse<GetStatusesFromProjectResponse>();
        Utils.Project.AssertGetStatusesResponse(response, result);
    }

    [Fact]
    public async Task GetStatuses_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetStatusesFromProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<GetStatusesFromProjectQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetStatuses(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetStatusesFromProjectQuery>(q =>
                Utils.Project.AssertGetStatusesFromProjectQuery(q, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task GetAllowedMembers_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetAllowedMembersFromProjectRequest();

        var result = ProjectResultFactory.CreateGetAllowedMembersFromProjectResult();
        _mediator
            .Send(Arg.Any<GetAllowedMembersFromProjectQuery>())
            .Returns(result);

        // Act
        var outcome = await _uut.GetAllowedMembers(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetAllowedMembersFromProjectQuery>(q =>
                Utils.Project.AssertGetAllowedMembersFromProjectQuery(q, request)));

        var response = outcome.ToResponse<GetAllowedMembersFromProjectResponse>();
        Utils.Project.AssertGetAllowedMembersResponse(response, result);
    }

    [Fact]
    public async Task GetAllowedMembers_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetAllowedMembersFromProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<GetAllowedMembersFromProjectQuery>())
            .Returns(error);

        // Act
        var outcome = await _uut.GetAllowedMembers(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<GetAllowedMembersFromProjectQuery>(q =>
                Utils.Project.AssertGetAllowedMembersFromProjectQuery(q, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Create_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateCreateProjectRequest();

        var result = ProjectResultFactory.CreateCreateProjectResult();
        _mediator
            .Send(Arg.Any<CreateProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateProjectCommand>(command => Utils.Project.AssertCreateCommand(command, request)));

        var response = outcome.ToResponse<CreateProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Create_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateCreateProjectRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<CreateProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Create(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<CreateProjectCommand>(command => Utils.Project.AssertCreateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task AddStatus_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateAddStatusToProjectRequest();

        var result = ProjectResultFactory.CreateAddStatusToProjectResult();
        _mediator
            .Send(Arg.Any<AddStatusToProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.AddStatus(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<AddStatusToProjectCommand>(command =>
                Utils.Project.AssertAddStatusCommand(command, request)));

        var response = outcome.ToResponse<AddStatusToProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task AddStatus_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateAddStatusToProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<AddStatusToProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.AddStatus(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<AddStatusToProjectCommand>(command =>
                Utils.Project.AssertAddStatusCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task AddAllowedMember_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateAddAllowedMemberToProjectRequest();

        var result = ProjectResultFactory.CreateAddAllowedMemberToProjectResult();
        _mediator
            .Send(Arg.Any<AddAllowedMemberToProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.AddAllowedMember(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<AddAllowedMemberToProjectCommand>(command =>
                Utils.Project.AssertAddAllowedMemberCommand(command, request)));

        var response = outcome.ToResponse<AddAllowedMemberToProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task AddAllowedMember_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateAddAllowedMemberToProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<AddAllowedMemberToProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.AddAllowedMember(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<AddAllowedMemberToProjectCommand>(command =>
                Utils.Project.AssertAddAllowedMemberCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Update_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateProjectRequest();

        var result = ProjectResultFactory.CreateUpdateProjectResult();
        _mediator
            .Send(Arg.Any<UpdateProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateProjectCommand>(command => Utils.Project.AssertUpdateCommand(command, request)));

        var response = outcome.ToResponse<UpdateProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Update_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateProjectRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<UpdateProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Update(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateProjectCommand>(command => Utils.Project.AssertUpdateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task UpdateFavorites_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateFavoritesProjectRequest();

        var result = ProjectResultFactory.CreateUpdateFavoritesProjectResult();
        _mediator
            .Send(Arg.Any<UpdateFavoritesProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.UpdateFavorites(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateFavoritesProjectCommand>(command =>
                Utils.Project.AssertUpdateFavoritesCommand(command, request)));

        var response = outcome.ToResponse<UpdateFavoritesProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateFavorites_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateFavoritesProjectRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<UpdateFavoritesProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.UpdateFavorites(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateFavoritesProjectCommand>(command =>
                Utils.Project.AssertUpdateFavoritesCommand(command, request)));

        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task UpdatePrivacy_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdatePrivacyProjectRequest();

        var result = ProjectResultFactory.CreateUpdatePrivacyProjectResult();
        _mediator
            .Send(Arg.Any<UpdatePrivacyProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.UpdatePrivacy(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdatePrivacyProjectCommand>(command =>
                Utils.Project.AssertUpdatePrivacyCommand(command, request)));

        var response = outcome.ToResponse<UpdatePrivacyProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdatePrivacy_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdatePrivacyProjectRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<UpdatePrivacyProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.UpdatePrivacy(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdatePrivacyProjectCommand>(command =>
                Utils.Project.AssertUpdatePrivacyCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task UpdateOverview_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateOverviewProjectRequest();

        var result = ProjectResultFactory.CreateUpdateOverviewProjectResult();
        _mediator
            .Send(Arg.Any<UpdateOverviewProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.UpdateOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateOverviewProjectCommand>(command =>
                Utils.Project.AssertUpdateOverviewCommand(command, request)));

        var response = outcome.ToResponse<UpdateOverviewProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateOverview_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateOverviewProjectRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<UpdateOverviewProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.UpdateOverview(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateOverviewProjectCommand>(command =>
                Utils.Project.AssertUpdateOverviewCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task UpdateOwner_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateOwnerProjectRequest();

        var result = ProjectResultFactory.CreateUpdateOwnerProjectResult();
        _mediator
            .Send(Arg.Any<UpdateOwnerProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.UpdateOwner(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateOwnerProjectCommand>(command =>
                Utils.Project.AssertUpdateOwnerCommand(command, request)));

        var response = outcome.ToResponse<UpdateOwnerProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateOwner_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateOwnerProjectRequest();

        var error = Errors.Workspace.NotFound;
        _mediator
            .Send(Arg.Any<UpdateOwnerProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.UpdateOwner(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateOwnerProjectCommand>(command =>
                Utils.Project.AssertUpdateOwnerCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task UpdateStatus_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateStatusFromProjectRequest();

        var result = ProjectResultFactory.CreateUpdateStatusFromProjectResult();
        _mediator
            .Send(Arg.Any<UpdateStatusFromProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.UpdateStatus(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateStatusFromProjectCommand>(command =>
                Utils.Project.AssertUpdateStatusCommand(command, request)));

        var response = outcome.ToResponse<UpdateStatusFromProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task UpdateStatus_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateStatusFromProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<UpdateStatusFromProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.UpdateStatus(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<UpdateStatusFromProjectCommand>(command =>
                Utils.Project.AssertUpdateStatusCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task Delete_WhenIsSuccessful_ShouldReturnSuccess()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateDeleteProjectRequest();

        var result = ProjectResultFactory.CreateDeleteProjectResult();
        _mediator
            .Send(Arg.Any<DeleteProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectCommand>(command => Utils.Project.AssertDeleteCommand(command, request)));

        var response = outcome.ToResponse<DeleteProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task Delete_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateDeleteProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<DeleteProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.Delete(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<DeleteProjectCommand>(command => Utils.Project.AssertDeleteCommand(command, request)));

        outcome.ValidateError(error);
    }
    
    [Fact]
    public async Task LeavePrivate_WhenIsSuccessful_ShouldReturnSuccess()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateLeavePrivateProjectRequest();

        var result = ProjectResultFactory.CreateLeavePrivateProjectResult();
        _mediator
            .Send(Arg.Any<LeavePrivateProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.LeavePrivate(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<LeavePrivateProjectCommand>(command =>
                Utils.Project.AssertLeavePrivateCommand(command, request)));

        var response = outcome.ToResponse<LeavePrivateProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task LeavePrivate_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateLeavePrivateProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<LeavePrivateProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.LeavePrivate(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<LeavePrivateProjectCommand>(command => 
                Utils.Project.AssertLeavePrivateCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task RemoveAllowedMember_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateRemoveAllowedMemberFromProjectRequest();

        var result = ProjectResultFactory.CreateRemoveAllowedMemberFromProjectResult();
        _mediator
            .Send(Arg.Any<RemoveAllowedMemberFromProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.RemoveAllowedMember(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RemoveAllowedMemberFromProjectCommand>(command =>
                Utils.Project.AssertRemoveAllowedMemberCommand(command, request)));

        var response = outcome.ToResponse<RemoveAllowedMemberFromProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task RemoveAllowedMember_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateRemoveAllowedMemberFromProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<RemoveAllowedMemberFromProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.RemoveAllowedMember(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RemoveAllowedMemberFromProjectCommand>(command =>
                Utils.Project.AssertRemoveAllowedMemberCommand(command, request)));

        outcome.ValidateError(error);
    }

    [Fact]
    public async Task RemoveStatus_WhenIsSuccessful_ShouldReturnResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateRemoveStatusFromProjectRequest();

        var result = ProjectResultFactory.CreateRemoveStatusFromProjectResult();
        _mediator
            .Send(Arg.Any<RemoveStatusFromProjectCommand>())
            .Returns(result);

        // Act
        var outcome = await _uut.RemoveStatus(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RemoveStatusFromProjectCommand>(command =>
                Utils.Project.AssertRemoveStatusCommand(command, request)));

        var response = outcome.ToResponse<RemoveStatusFromProjectResponse>();
        response.Message.Should().Be(result.Message);
    }

    [Fact]
    public async Task RemoveStatus_WhenItFails_ShouldReturnProblem()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateRemoveStatusFromProjectRequest();

        var error = Errors.Project.NotFound;
        _mediator
            .Send(Arg.Any<RemoveStatusFromProjectCommand>())
            .Returns(error);

        // Act
        var outcome = await _uut.RemoveStatus(request, default);

        // Assert
        await _mediator
            .Received(1)
            .Send(Arg.Is<RemoveStatusFromProjectCommand>(command =>
                Utils.Project.AssertRemoveStatusCommand(command, request)));

        outcome.ValidateError(error);
    }
}