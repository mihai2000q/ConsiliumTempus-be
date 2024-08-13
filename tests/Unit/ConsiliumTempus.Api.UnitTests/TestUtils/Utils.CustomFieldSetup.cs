using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Entities;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class CustomFieldSetup
    {
        public static bool AssertGetCollectionCustomFieldSetupQuery(
            GetCollectionCustomFieldSetupQuery query,
            GetCollectionCustomFieldSetupFromProjectRequest request)
        {
            query.ProjectId.Should().Be(request.ProjectId);
            query.WorkspaceId.Should().BeNull();

            return true;
        }
        
        public static bool AssertCreateCustomFieldSetupCommand(
            CreateCustomFieldSetupCommand command,
            CreateCustomFieldSetupOnProjectRequest request)
        {
            command.WorkspaceId.Should().BeNull();
            command.ProjectId.Should().Be(request.ProjectId);
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            command.Type.Should().Be(request.Type);
            command.NumberSettings?.CurrencyCode.Should().Be(request.NumberSettings!.CurrencyCode);
            command.NumberSettings?.Decimals.Should().Be(request.NumberSettings!.Decimals);
            command.NumberSettings?.Rounding.Should().Be(request.NumberSettings!.Rounding);
            command.SingleSelectOptions
                ?.Zip(request.SingleSelectOptions!)
                .Should().AllSatisfy(x => AssertSingleSelectOptionCommand(x.First, x.Second));

            return true;
        }
        
        public static void AssertGetCollectionCustomFieldSetupFromProjectResponse(
            GetCollectionCustomFieldSetupFromProjectResponse response,
            GetCollectionCustomFieldSetupResult result)
        {
            response.CustomFieldSetups
                .Zip(result.CustomFieldSetups)
                .Should().AllSatisfy(x => AssertCustomFieldSetup(x.First, x.Second));
        }

        private static void AssertCustomFieldSetup(
            GetCollectionCustomFieldSetupFromProjectResponse.CustomFieldSetupResponse response,
            CustomFieldSetupAggregate customFieldSetup)
        {
            response.Id.Should().Be(customFieldSetup.Id.Value);
            response.Name.Should().Be(customFieldSetup.Name.Value);
            response.Description.Should().Be(customFieldSetup.Description.Value);

            switch (response)
            {
                case GetCollectionCustomFieldSetupFromProjectResponse.NumberCustomFieldSetupResponse numberResponse:
                {
                    var numberCustomFieldSetup = (NumberCustomFieldSetupAggregate)customFieldSetup;
                    numberResponse.Settings.CurrencyCode.Should().Be(numberCustomFieldSetup.Settings.CurrencyCode);
                    numberResponse.Settings.Decimals.Should().Be(numberCustomFieldSetup.Settings.Decimals);
                    numberResponse.Settings.Rounding.Should().Be(numberCustomFieldSetup.Settings.Rounding);
                    break;
                }
                case GetCollectionCustomFieldSetupFromProjectResponse.SingleSelectCustomFieldSetupResponse
                    singleSelectResponse:
                {
                    var singleSelectCustomFieldSetup = (SingleSelectCustomFieldSetupAggregate)customFieldSetup;
                    singleSelectResponse.Options
                        .Zip(singleSelectCustomFieldSetup.Options)
                        .Should().AllSatisfy(x => AssertSingleSelectOptionResponse(x.First, x.Second));
                    break;
                }
                case GetCollectionCustomFieldSetupFromProjectResponse.TextCustomFieldSetupResponse:
                    break;
            }
        }

        private static void AssertSingleSelectOptionResponse(
            GetCollectionCustomFieldSetupFromProjectResponse.SingleSelectCustomFieldSetupResponse.SingleSelectOptionResponse response,
            SingleSelectOption singleSelectOption)
        {
            response.Id.Should().Be(singleSelectOption.Id);
            response.Value.Should().Be(singleSelectOption.Value);
            response.Color.Should().Be(singleSelectOption.Color);
        }

        private static void AssertSingleSelectOptionCommand(
            CreateCustomFieldSetupCommand.SingleSelectOptionCommand singleSelectOptionCommand,
            CreateCustomFieldSetupOnProjectRequest.SingleSelectOptionRequest singleSelectOptionRequest)
        {
            singleSelectOptionCommand.Value.Should().Be(singleSelectOptionRequest.Value);
            singleSelectOptionCommand.Color.Should().Be(singleSelectOptionRequest.Color);
        }
    }
}