using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.RemoveFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.UpdateWorkspace;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.UpdateWorkspace;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using Mapster;

namespace ConsiliumTempus.Api.Common.Mapping;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class CustomFieldSetupMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        GetMappings(config);
        GetCollectionFromProjectMappings(config);
        GetCollectionFromWorkspaceMappings(config);
        CreateOnWorkspaceMappings(config);
        CreateOnProjectMappings(config);
        UpdateWorkspaceMappings(config);
        DeleteMappings(config);
        RemoveFromProjectMappings(config);
    }

    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCustomFieldSetupRequest, GetCustomFieldSetupQuery>();

        config.NewConfig<GetCustomFieldSetupResult, GetCustomFieldSetupResponse>();
        config.NewConfig<NumberCustomFieldSetupAggregate, GetCustomFieldSetupResponse.NumberCustomFieldSetupResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.DefaultNumber, src => src.DefaultNumber!.Value)
            .Map(dest => dest.Type, src => CustomFieldType.Number.ToString());
        config
            .NewConfig<SingleSelectCustomFieldSetupAggregate,
                GetCustomFieldSetupResponse.SingleSelectCustomFieldSetupResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src => CustomFieldType.SingleSelect.ToString());
        config.NewConfig<TextCustomFieldSetupAggregate, GetCustomFieldSetupResponse.TextCustomFieldSetupResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.DefaultText, src => src.DefaultText!.Value)
            .Map(dest => dest.Type, src => CustomFieldType.Text.ToString());

        config.NewConfig<CustomFieldSetupAggregate, GetCustomFieldSetupResponse.CustomFieldSetupResponse>()
            .MapWith(src => Convert(src));
    }

    private static GetCustomFieldSetupResponse.CustomFieldSetupResponse Convert(CustomFieldSetupAggregate setup)
    {
        return setup switch
        {
            NumberCustomFieldSetupAggregate numberSetup =>
                numberSetup.Adapt<GetCustomFieldSetupResponse.NumberCustomFieldSetupResponse>(),

            SingleSelectCustomFieldSetupAggregate singleSelectSetup =>
                singleSelectSetup.Adapt<GetCustomFieldSetupResponse.SingleSelectCustomFieldSetupResponse>(),

            TextCustomFieldSetupAggregate textSetup =>
                textSetup.Adapt<GetCustomFieldSetupResponse.TextCustomFieldSetupResponse>(),

            _ => throw new ArgumentOutOfRangeException(nameof(setup), setup, null)
        };
    }

    private static void GetCollectionFromProjectMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionCustomFieldSetupFromProjectRequest, GetCollectionCustomFieldSetupQuery>();

        var setupTypeToCustomFieldType = new Dictionary<Type, CustomFieldType>
        {
            { typeof(NumberCustomFieldSetupAggregate), CustomFieldType.Number },
            { typeof(SingleSelectCustomFieldSetupAggregate), CustomFieldType.SingleSelect },
            { typeof(TextCustomFieldSetupAggregate), CustomFieldType.Text },
        };

        config.NewConfig<GetCollectionCustomFieldSetupResult, GetCollectionCustomFieldSetupFromProjectResponse>();
        config.NewConfig<CustomFieldSetupAggregate,
                GetCollectionCustomFieldSetupFromProjectResponse.CustomFieldSetupResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src => setupTypeToCustomFieldType[src.GetType()]);
    }

    private static void GetCollectionFromWorkspaceMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionCustomFieldSetupFromWorkspaceRequest, GetCollectionCustomFieldSetupQuery>();

        var setupTypeToCustomFieldType = new Dictionary<Type, CustomFieldType>
        {
            { typeof(NumberCustomFieldSetupAggregate), CustomFieldType.Number },
            { typeof(SingleSelectCustomFieldSetupAggregate), CustomFieldType.SingleSelect },
            { typeof(TextCustomFieldSetupAggregate), CustomFieldType.Text },
        };

        config.NewConfig<GetCollectionCustomFieldSetupResult, GetCollectionCustomFieldSetupFromWorkspaceResponse>();
        config.NewConfig<CustomFieldSetupAggregate, 
                GetCollectionCustomFieldSetupFromWorkspaceResponse.CustomFieldSetupResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src => setupTypeToCustomFieldType[src.GetType()]);
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

    private static void UpdateWorkspaceMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateWorkspaceCustomFieldSetupRequest, UpdateWorkspaceCustomFieldSetupCommand>();

        config.NewConfig<UpdateWorkspaceCustomFieldSetupResult, UpdateWorkspaceCustomFieldSetupResponse>();
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