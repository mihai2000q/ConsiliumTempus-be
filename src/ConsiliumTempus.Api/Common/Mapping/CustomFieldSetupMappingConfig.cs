using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
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
        CreateOnProjectMappings(config);
    }
    
    private static void GetMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCustomFieldSetupRequest, GetCustomFieldSetupQuery>();

        config.NewConfig<GetCollectionCustomFieldSetupResult, GetCustomFieldSetupResponse>();
        config.NewConfig<CustomFieldSetupAggregate,
                GetCustomFieldSetupResponse.CustomFieldSetupResponse>()
            .Include<NumberCustomFieldSetupAggregate,
                GetCustomFieldSetupResponse.NumberCustomFieldSetupResponse>()
            .Include<SingleSelectCustomFieldSetupAggregate,
                GetCustomFieldSetupResponse.SingleSelectCustomFieldSetupResponse>()
            .Include<TextCustomFieldSetupAggregate,
                GetCustomFieldSetupResponse.TextCustomFieldSetupResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value);

        config.NewConfig<NumberCustomFieldSetupAggregate, GetCustomFieldSetupResponse.NumberCustomFieldSetupResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.DefaultNumber, 
                src => src.DefaultNumber!.Value);
        config.NewConfig<TextCustomFieldSetupAggregate, GetCustomFieldSetupResponse.TextCustomFieldSetupResponse>()
            .IgnoreNullValues(true)
            .Map(dest => dest.DefaultText, 
                src => src.DefaultText!.Value);
    }

    private static void GetCollectionFromProjectMappings(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionCustomFieldSetupFromProjectRequest, GetCollectionCustomFieldSetupQuery>();

        config.NewConfig<GetCollectionCustomFieldSetupResult, GetCollectionCustomFieldSetupFromProjectResponse>();
        config.NewConfig<CustomFieldSetupAggregate,
                GetCollectionCustomFieldSetupFromProjectResponse.CustomFieldSetupResponse>()
            .Include<NumberCustomFieldSetupAggregate,
                GetCollectionCustomFieldSetupFromProjectResponse.NumberCustomFieldSetupResponse>()
            .Include<SingleSelectCustomFieldSetupAggregate,
                GetCollectionCustomFieldSetupFromProjectResponse.SingleSelectCustomFieldSetupResponse>()
            .Include<TextCustomFieldSetupAggregate,
                GetCollectionCustomFieldSetupFromProjectResponse.TextCustomFieldSetupResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value);
    }

    private static void CreateOnProjectMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCustomFieldSetupOnProjectRequest, CreateCustomFieldSetupCommand>();

        config.NewConfig<CreateCustomFieldSetupResult, CreateCustomFieldSetupOnProjectResponse>();
    }
}