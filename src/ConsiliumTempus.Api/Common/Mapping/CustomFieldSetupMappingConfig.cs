using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
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
        GetCollectionFromProject(config);
        CreateOnProjectMappings(config);
    }

    private static void GetCollectionFromProject(TypeAdapterConfig config)
    {
        config.NewConfig<GetCollectionCustomFieldSetupFromProjectRequest, GetCollectionCustomFieldSetupQuery>();

        config.NewConfig<GetCollectionCustomFieldSetupResult, GetCollectionCustomFieldSetupFromProjectResponse>();
        config.NewConfig<CustomFieldSetupAggregate,
                GetCollectionCustomFieldSetupFromProjectResponse.CustomFieldSetupResponse>()
            .Include<NumberCustomFieldSetupAggregate, GetCollectionCustomFieldSetupFromProjectResponse.NumberCustomFieldSetupResponse>()
            .Include<SingleSelectCustomFieldSetupAggregate, GetCollectionCustomFieldSetupFromProjectResponse.SingleSelectCustomFieldSetupResponse>()
            .Include<TextCustomFieldSetupAggregate, GetCollectionCustomFieldSetupFromProjectResponse.TextCustomFieldSetupResponse>()
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