using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
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
        CreateOnProjectMappings(config);
        DeleteMappings(config);
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

        config.NewConfig<GetCollectionCustomFieldSetupResult, GetCollectionCustomFieldSetupFromProjectResponse>();
        config.NewConfig<CustomFieldSetupAggregate,
                GetCollectionCustomFieldSetupFromProjectResponse.CustomFieldSetupResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description.Value)
            .Map(dest => dest.Type, src =>
                src is NumberCustomFieldSetupAggregate
                    ? CustomFieldType.Number
                    : src is SingleSelectCustomFieldSetupAggregate
                        ? CustomFieldType.SingleSelect
                        : CustomFieldType.Text);
    }

    private static void CreateOnProjectMappings(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCustomFieldSetupOnProjectRequest, CreateCustomFieldSetupCommand>();

        config.NewConfig<CreateCustomFieldSetupResult, CreateCustomFieldSetupOnProjectResponse>();
    }

    private static void DeleteMappings(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteCustomFieldSetupRequest, DeleteCustomFieldSetupCommand>();

        config.NewConfig<DeleteCustomFieldSetupResult, DeleteCustomFieldSetupResponse>();
    }
}