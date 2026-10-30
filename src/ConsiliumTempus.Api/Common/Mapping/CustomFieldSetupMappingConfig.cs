using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.MakeGlobal;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.RemoveFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Update;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.MakeGlobal;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
internal sealed class CustomFieldSetupMappingConfig : IRegister
{
    private readonly Dictionary<Type, CustomFieldType> _setupTypeToCustomFieldType = new()
    {
        { typeof(DateCustomFieldSetupAggregate), CustomFieldType.Date },
        { typeof(DateTimeCustomFieldSetupAggregate), CustomFieldType.DateTime },
        { typeof(DurationCustomFieldSetupAggregate), CustomFieldType.Duration },
        { typeof(MultiSelectCustomFieldSetupAggregate), CustomFieldType.MultiSelect },
        { typeof(NumberCustomFieldSetupAggregate), CustomFieldType.Number },
        { typeof(PeopleCustomFieldSetupAggregate), CustomFieldType.People },
        { typeof(SingleSelectCustomFieldSetupAggregate), CustomFieldType.SingleSelect },
        { typeof(TextCustomFieldSetupAggregate), CustomFieldType.Text },
        { typeof(TimeCustomFieldSetupAggregate), CustomFieldType.Time },
    };

    public void Register(TypeAdapterConfig config)
    {
        GetMappings(config);
        GetCollectionFromProjectMappings(config);
        GetCollectionFromWorkspaceMappings(config);
        CreateOnWorkspaceMappings(config);
        CreateOnProjectMappings(config);
        UpdateMappings(config);
        MakeGlobalMappings(config);
        DeleteMappings(config);
        RemoveFromProjectMappings(config);
    }

    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCustomFieldSetupRequest, GetCustomFieldSetupQuery>();

        config.NewConfig<GetCustomFieldSetupResult, GetCustomFieldSetupResponse>()
            .MapWith(src => Convert(src.CustomFieldSetup));
        config.NewConfig<DateCustomFieldSetupAggregate, GetCustomFieldSetupResponse.GetDateCustomFieldSetupResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src => CustomFieldType.Date);
        config.NewConfig<DateTimeCustomFieldSetupAggregate, GetCustomFieldSetupResponse.GetDateTimeCustomFieldSetupResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src => CustomFieldType.DateTime);
        config.NewConfig<DurationCustomFieldSetupAggregate, GetCustomFieldSetupResponse.GetDurationCustomFieldSetupResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src => CustomFieldType.Duration);
        config.NewConfig<MultiSelectCustomFieldSetupAggregate, GetCustomFieldSetupResponse.GetMultiSelectCustomFieldSetupResponse>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src => CustomFieldType.MultiSelect);
        config.NewConfig<NumberCustomFieldSetupAggregate, GetCustomFieldSetupResponse.GetNumberCustomFieldSetupResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.DefaultNumber, src => src.DefaultNumber!.Value)
            .Map(dest => dest.Type, src => CustomFieldType.Number);
        config.NewConfig<PeopleCustomFieldSetupAggregate, GetCustomFieldSetupResponse.GetPeopleCustomFieldSetupResponse>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src => CustomFieldType.People);
        config
            .NewConfig<SingleSelectCustomFieldSetupAggregate, 
                GetCustomFieldSetupResponse.GetSingleSelectCustomFieldSetupResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src => CustomFieldType.SingleSelect);
        config.NewConfig<TextCustomFieldSetupAggregate, GetCustomFieldSetupResponse.GetTextCustomFieldSetupResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.DefaultText, src => src.DefaultText!.Value)
            .Map(dest => dest.Type, src => CustomFieldType.Text);
        config.NewConfig<TimeCustomFieldSetupAggregate, GetCustomFieldSetupResponse.GetTimeCustomFieldSetupResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src => CustomFieldType.Time);

    }

    private static GetCustomFieldSetupResponse Convert(CustomFieldSetupAggregate setup)
    {
        return setup switch
        {
            DateCustomFieldSetupAggregate dateSetup =>
                dateSetup.Adapt<GetCustomFieldSetupResponse.GetDateCustomFieldSetupResponse>(),
            
            DateTimeCustomFieldSetupAggregate dateTimeSetup =>
                dateTimeSetup.Adapt<GetCustomFieldSetupResponse.GetDateTimeCustomFieldSetupResponse>(),
            
            DurationCustomFieldSetupAggregate durationSetup =>
                durationSetup.Adapt<GetCustomFieldSetupResponse.GetDurationCustomFieldSetupResponse>(),
            
            MultiSelectCustomFieldSetupAggregate multiSelectSetup =>
                multiSelectSetup.Adapt<GetCustomFieldSetupResponse.GetMultiSelectCustomFieldSetupResponse>(),
            
            NumberCustomFieldSetupAggregate numberSetup =>
                numberSetup.Adapt<GetCustomFieldSetupResponse.GetNumberCustomFieldSetupResponse>(),
            
            PeopleCustomFieldSetupAggregate peopleSetup =>
                peopleSetup.Adapt<GetCustomFieldSetupResponse.GetPeopleCustomFieldSetupResponse>(),

            SingleSelectCustomFieldSetupAggregate singleSelectSetup =>
                singleSelectSetup.Adapt<GetCustomFieldSetupResponse.GetSingleSelectCustomFieldSetupResponse>(),

            TextCustomFieldSetupAggregate textSetup =>
                textSetup.Adapt<GetCustomFieldSetupResponse.GetTextCustomFieldSetupResponse>(),
            
            TimeCustomFieldSetupAggregate timeSetup =>
                timeSetup.Adapt<GetCustomFieldSetupResponse.GetTimeCustomFieldSetupResponse>(),

            _ => throw new ArgumentOutOfRangeException(nameof(setup), setup, null)
        };
    }

    private void GetCollectionFromProjectMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionCustomFieldSetupFromProjectRequest, GetCollectionCustomFieldSetupQuery>();

        config.NewConfig<GetCollectionCustomFieldSetupResult, GetCollectionCustomFieldSetupFromProjectResponse>();
        config.NewConfig<CustomFieldSetupAggregate,
                GetCollectionCustomFieldSetupFromProjectResponse.CustomFieldSetupResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src => _setupTypeToCustomFieldType[src.GetType()]);
    }

    private void GetCollectionFromWorkspaceMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionCustomFieldSetupFromWorkspaceRequest, GetCollectionCustomFieldSetupQuery>();

        config.NewConfig<GetCollectionCustomFieldSetupResult, GetCollectionCustomFieldSetupFromWorkspaceResponse>();
        config.NewConfig<CustomFieldSetupAggregate,
                GetCollectionCustomFieldSetupFromWorkspaceResponse.CustomFieldSetupResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src => _setupTypeToCustomFieldType[src.GetType()]);
    }

    private static void CreateOnWorkspaceMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCustomFieldSetupOnWorkspaceRequest, CreateCustomFieldSetupCommand>();

        config.NewConfig<CreateCustomFieldSetupResult, CreateCustomFieldSetupOnWorkspaceResponse>();
    }

    private static void CreateOnProjectMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCustomFieldSetupOnProjectRequest, CreateCustomFieldSetupCommand>();

        config.NewConfig<CreateCustomFieldSetupResult, CreateCustomFieldSetupOnProjectResponse>();
    }

    private static void UpdateMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateCustomFieldSetupRequest, UpdateCustomFieldSetupCommand>();

        config.NewConfig<UpdateCustomFieldSetupResult, UpdateCustomFieldSetupResponse>();
    }

    private static void MakeGlobalMappings(TypeAdapterConfig config)
    {
        config.NewConfig<MakeCustomFieldSetupGlobalRequest, MakeCustomFieldSetupGlobalCommand>();

        config.NewConfig<MakeCustomFieldSetupGlobalResult, MakeCustomFieldGlobalResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteCustomFieldSetupRequest, DeleteCustomFieldSetupCommand>();

        config.NewConfig<DeleteCustomFieldSetupResult, DeleteCustomFieldSetupResponse>();
    }

    private static void RemoveFromProjectMappings(TypeAdapterConfig config)
    {
        config.NewConfig<RemoveCustomFieldSetupFromProjectRequest, RemoveCustomFieldSetupFromProjectCommand>();

        config.NewConfig<RemoveCustomFieldSetupFromProjectResult, RemoveCustomFieldSetupFromProjectResponse>();
    }
}