using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectTask.Update;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateCustomField;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateIsCompleted;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateOverview;
using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateIsCompleted;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ProjectTaskMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        GetMappings(config);
        GetCollectionMappings(config);
        CreateMappings(config);
        UpdateMappings(config);
        UpdateCustomFieldMappings(config);
        UpdateIsCompletedMappings(config);
        UpdateOverviewMappings(config);
        DeleteMappings(config);
    }

    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetProjectTaskRequest, GetProjectTaskQuery>();

        config.NewConfig<ProjectTaskAggregate, GetProjectTaskResponse>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.IsCompleted, src => src.IsCompleted.Value)
            .Map(dest => dest.Sprint, src => src.Stage.Sprint)
            .Map(dest => dest.Project, src => src.Stage.Sprint.Project)
            .Map(dest => dest.Workspace, src => src.Stage.Sprint.Project.Workspace);

        config.NewConfig<UserAggregate, GetProjectTaskResponse.UserResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Email, src => src.Credentials.Email);
        config.NewConfig<ProjectStage, GetProjectTaskResponse.ProjectStageResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);
        config.NewConfig<ProjectSprintAggregate, GetProjectTaskResponse.ProjectSprintResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);
        config.NewConfig<ProjectAggregate, GetProjectTaskResponse.ProjectResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);
        config.NewConfig<WorkspaceAggregate, GetProjectTaskResponse.WorkspaceResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<DateCustomField, GetProjectTaskResponse.DateCustomFieldResponse>()
            .ConstructUsing(src => new GetProjectTaskResponse.DateCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.Date,
                src.Date));
        config.NewConfig<DateTimeCustomField, GetProjectTaskResponse.DateTimeCustomFieldResponse>()
            .ConstructUsing(src => new GetProjectTaskResponse.DateTimeCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.DateTime,
                src.DateTime));
        config.NewConfig<DurationCustomField, GetProjectTaskResponse.DurationCustomFieldResponse>()
            .ConstructUsing(src => new GetProjectTaskResponse.DurationCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.Duration,
                src.Duration));
        config.NewConfig<MultiSelectCustomField, GetProjectTaskResponse.MultiSelectCustomFieldResponse>()
            .ConstructUsing(src => new GetProjectTaskResponse.MultiSelectCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.MultiSelect,
                src.Options.Adapt<List<GetProjectTaskResponse.MultiSelectCustomFieldResponse.
                    MultiSelectOptionResponse>>(),
                src.Setup.Options.Adapt<List<GetProjectTaskResponse.MultiSelectCustomFieldResponse.
                    MultiSelectOptionResponse>>()));
        config.NewConfig<NumberCustomField, GetProjectTaskResponse.NumberCustomFieldResponse>()
            .ConstructUsing(src => new GetProjectTaskResponse.NumberCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.Number,
                src.Number == null ? null : src.Number.Value));
        config.NewConfig<PeopleCustomField, GetProjectTaskResponse.PeopleCustomFieldResponse>()
            .ConstructUsing(src => new GetProjectTaskResponse.PeopleCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.People,
                src.Person.Adapt<GetProjectTaskResponse.UserResponse>()));
        config.NewConfig<SingleSelectCustomField, GetProjectTaskResponse.SingleSelectCustomFieldResponse>()
            .ConstructUsing(src => new GetProjectTaskResponse.SingleSelectCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.SingleSelect,
                src.Option == null
                    ? null
                    : src.Option.Adapt<GetProjectTaskResponse.SingleSelectCustomFieldResponse
                        .SingleSelectOptionResponse>(),
                src.Setup.Options.Adapt<List<GetProjectTaskResponse.SingleSelectCustomFieldResponse
                    .SingleSelectOptionResponse>>()));
        config.NewConfig<TextCustomField, GetProjectTaskResponse.TextCustomFieldResponse>()
            .ConstructUsing(src => new GetProjectTaskResponse.TextCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.Text,
                src.Text == null ? null : src.Text.Value));
        config.NewConfig<TimeCustomField, GetProjectTaskResponse.TimeCustomFieldResponse>()
            .ConstructUsing(src => new GetProjectTaskResponse.TimeCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.Time,
                src.Time));

        config.NewConfig<CustomField, GetProjectTaskResponse.CustomFieldResponse>()
            .MapWith(src => Convert(src));
    }

    private static GetProjectTaskResponse.CustomFieldResponse Convert(CustomField customField)
    {
        return customField switch
        {
            DateCustomField dateCustomField =>
                dateCustomField.Adapt<GetProjectTaskResponse.DateCustomFieldResponse>(),

            DateTimeCustomField dateTimeCustomField =>
                dateTimeCustomField.Adapt<GetProjectTaskResponse.DateTimeCustomFieldResponse>(),

            DurationCustomField durationCustomField =>
                durationCustomField.Adapt<GetProjectTaskResponse.DurationCustomFieldResponse>(),

            MultiSelectCustomField multiSelectCustomField =>
                multiSelectCustomField.Adapt<GetProjectTaskResponse.MultiSelectCustomFieldResponse>(),

            NumberCustomField numberCustomField =>
                numberCustomField.Adapt<GetProjectTaskResponse.NumberCustomFieldResponse>(),

            PeopleCustomField peopleCustomField =>
                peopleCustomField.Adapt<GetProjectTaskResponse.PeopleCustomFieldResponse>(),

            SingleSelectCustomField singleSelectCustomField =>
                singleSelectCustomField.Adapt<GetProjectTaskResponse.SingleSelectCustomFieldResponse>(),

            TextCustomField textCustomField =>
                textCustomField.Adapt<GetProjectTaskResponse.TextCustomFieldResponse>(),

            TimeCustomField timeCustomField =>
                timeCustomField.Adapt<GetProjectTaskResponse.TimeCustomFieldResponse>(),

            _ => throw new ArgumentOutOfRangeException(nameof(customField), customField, "Type Not Supported")
        };
    }

    private static void GetCollectionMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionProjectTaskRequest, GetCollectionProjectTaskQuery>();

        config.NewConfig<GetCollectionProjectTaskResult, GetCollectionProjectTaskResponse>();
        config.NewConfig<ProjectTaskAggregate, GetCollectionProjectTaskResponse.ProjectTaskResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.IsCompleted, src => src.IsCompleted.Value);
        config.NewConfig<UserAggregate, GetCollectionProjectTaskResponse.UserResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Email, src => src.Credentials.Email);

        config.NewConfig<DateCustomField, GetCollectionProjectTaskResponse.DateCustomFieldResponse>()
            .ConstructUsing(src => new GetCollectionProjectTaskResponse.DateCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.Date,
                src.Date));
        config.NewConfig<DateTimeCustomField, GetCollectionProjectTaskResponse.DateTimeCustomFieldResponse>()
            .ConstructUsing(src => new GetCollectionProjectTaskResponse.DateTimeCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.DateTime,
                src.DateTime));
        config.NewConfig<DurationCustomField, GetCollectionProjectTaskResponse.DurationCustomFieldResponse>()
            .ConstructUsing(src => new GetCollectionProjectTaskResponse.DurationCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.Duration,
                src.Duration));
        config.NewConfig<MultiSelectCustomField, GetCollectionProjectTaskResponse.MultiSelectCustomFieldResponse>()
            .ConstructUsing(src => new GetCollectionProjectTaskResponse.MultiSelectCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.MultiSelect,
                src.Options.Adapt<List<GetCollectionProjectTaskResponse.MultiSelectCustomFieldResponse.
                    MultiSelectOptionResponse>>()));
        config.NewConfig<NumberCustomField, GetCollectionProjectTaskResponse.NumberCustomFieldResponse>()
            .ConstructUsing(src => new GetCollectionProjectTaskResponse.NumberCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.Number,
                src.Number == null ? null : src.Number.Value));
        config.NewConfig<PeopleCustomField, GetCollectionProjectTaskResponse.PeopleCustomFieldResponse>()
            .ConstructUsing(src => new GetCollectionProjectTaskResponse.PeopleCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.People,
                src.Person.Adapt<GetCollectionProjectTaskResponse.UserResponse>()));
        config.NewConfig<SingleSelectCustomField, GetCollectionProjectTaskResponse.SingleSelectCustomFieldResponse>()
            .ConstructUsing(src => new GetCollectionProjectTaskResponse.SingleSelectCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.SingleSelect,
                src.Option == null
                    ? null
                    : src.Option.Adapt<GetCollectionProjectTaskResponse.SingleSelectCustomFieldResponse.
                        SingleSelectOptionResponse>()));
        config.NewConfig<TextCustomField, GetCollectionProjectTaskResponse.TextCustomFieldResponse>()
            .ConstructUsing(src => new GetCollectionProjectTaskResponse.TextCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.Text,
                src.Text == null ? null : src.Text.Value));
        config.NewConfig<TimeCustomField, GetCollectionProjectTaskResponse.TimeCustomFieldResponse>()
            .ConstructUsing(src => new GetCollectionProjectTaskResponse.TimeCustomFieldResponse(
                src.Id.Value,
                src.Setup.Name.Value,
                src.Setup.Description.Value,
                CustomFieldType.Time,
                src.Time));

        config.NewConfig<CustomField, GetCollectionProjectTaskResponse.CustomFieldResponse>()
            .MapWith(src => ConvertCollection(src));
    }

    private static GetCollectionProjectTaskResponse.CustomFieldResponse ConvertCollection(CustomField customField)
    {
        return customField switch
        {
            DateCustomField dateCustomField =>
                dateCustomField.Adapt<GetCollectionProjectTaskResponse.DateCustomFieldResponse>(),

            DateTimeCustomField dateTimeCustomField =>
                dateTimeCustomField.Adapt<GetCollectionProjectTaskResponse.DateTimeCustomFieldResponse>(),

            DurationCustomField durationCustomField =>
                durationCustomField.Adapt<GetCollectionProjectTaskResponse.DurationCustomFieldResponse>(),

            MultiSelectCustomField multiSelectCustomField =>
                multiSelectCustomField.Adapt<GetCollectionProjectTaskResponse.MultiSelectCustomFieldResponse>(),

            NumberCustomField numberCustomField =>
                numberCustomField.Adapt<GetCollectionProjectTaskResponse.NumberCustomFieldResponse>(),

            PeopleCustomField peopleCustomField =>
                peopleCustomField.Adapt<GetCollectionProjectTaskResponse.PeopleCustomFieldResponse>(),

            SingleSelectCustomField singleSelectCustomField =>
                singleSelectCustomField.Adapt<GetCollectionProjectTaskResponse.SingleSelectCustomFieldResponse>(),

            TextCustomField textCustomField =>
                textCustomField.Adapt<GetCollectionProjectTaskResponse.TextCustomFieldResponse>(),

            TimeCustomField timeCustomField =>
                timeCustomField.Adapt<GetCollectionProjectTaskResponse.TimeCustomFieldResponse>(),

            _ => throw new ArgumentOutOfRangeException(nameof(customField), customField, "Type Not Supported")
        };
    }

    private static void CreateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectTaskRequest, CreateProjectTaskCommand>();

        config.NewConfig<CreateProjectTaskResult, CreateProjectTaskResponse>();
    }

    private static void UpdateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateProjectTaskRequest, UpdateProjectTaskCommand>();

        config.NewConfig<UpdateProjectTaskResult, UpdateProjectTaskResponse>();
    }

    private static void UpdateCustomFieldMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateCustomFieldFromProjectTaskRequest, UpdateCustomFieldFromProjectTaskCommand>();

        config.NewConfig<UpdateCustomFieldFromProjectTaskResult, UpdateCustomFieldFromProjectTaskResponse>();
    }

    private static void UpdateIsCompletedMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateIsCompletedProjectTaskRequest, UpdateIsCompletedProjectTaskCommand>();

        config.NewConfig<UpdateIsCompletedProjectTaskResult, UpdateIsCompletedProjectTaskResponse>();
    }

    private static void UpdateOverviewMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateOverviewProjectTaskRequest, UpdateOverviewProjectTaskCommand>();

        config.NewConfig<UpdateOverviewProjectTaskResult, UpdateOverviewProjectTaskResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteProjectTaskRequest, DeleteProjectTaskCommand>();

        config.NewConfig<DeleteProjectTaskResult, DeleteProjectTaskResponse>();
    }
}