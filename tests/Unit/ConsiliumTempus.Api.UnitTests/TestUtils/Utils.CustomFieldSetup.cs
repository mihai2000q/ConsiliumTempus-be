using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class CustomFieldSetup
    {
        public static bool AssertGetCustomFieldSetupQuery(
            GetCustomFieldSetupQuery query,
            GetCustomFieldSetupRequest request)
        {
            query.Id.Should().Be(request.Id);

            return true;
        }

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

            command.NumberCustomFieldSetup?.Settings.CurrencyCode
                .Should().Be(request.NumberCustomFieldSetup!.Settings.CurrencyCode);
            command.NumberCustomFieldSetup?.Settings.Decimals
                .Should().Be(request.NumberCustomFieldSetup!.Settings.Decimals);
            command.NumberCustomFieldSetup?.Settings.Rounding
                .Should().Be(request.NumberCustomFieldSetup!.Settings.Rounding);
            command.NumberCustomFieldSetup?.DefaultNumber.Should().Be(request.NumberCustomFieldSetup!.DefaultNumber);

            command.SingleSelectCustomFieldSetup?.Options
                .Zip(request.SingleSelectCustomFieldSetup!.Options)
                .Should().AllSatisfy(x => AssertSingleSelectOptionCommand(x.First, x.Second));
            command.SingleSelectCustomFieldSetup?.DefaultOptionId.Should()
                .Be(request.SingleSelectCustomFieldSetup!.DefaultOptionId);

            command.TextCustomFieldSetup?.DefaultText.Should().Be(request.TextCustomFieldSetup!.DefaultText);

            return true;
        }

        public static void AssertGetCustomFieldSetupResponse(
            GetCustomFieldSetupResponse response,
            GetCustomFieldSetupResult result)
        {
            response.CustomFieldSetup.Id.Should().Be(result.CustomFieldSetup.Id.Value);
            response.CustomFieldSetup.Name.Should().Be(result.CustomFieldSetup.Name.Value);
            response.CustomFieldSetup.Description.Should().Be(result.CustomFieldSetup.Description.Value);
            switch (response.CustomFieldSetup)
            {
                case GetCustomFieldSetupResponse.NumberCustomFieldSetupResponse numberSetup:
                    AssertNumberCustomFieldSetupResponse(numberSetup,
                        (NumberCustomFieldSetupAggregate)result.CustomFieldSetup);
                    break;
                case GetCustomFieldSetupResponse.SingleSelectCustomFieldSetupResponse singleSelectSetup:
                    AssertSingleSelectCustomFieldSetupResponse(singleSelectSetup,
                        (SingleSelectCustomFieldSetupAggregate)result.CustomFieldSetup);
                    break;
                case GetCustomFieldSetupResponse.TextCustomFieldSetupResponse textSetup:
                    AssertTextCustomFieldSetupResponse(textSetup, 
                        (TextCustomFieldSetupAggregate)result.CustomFieldSetup);
                    break;
            }
        }

        public static void AssertGetCollectionCustomFieldSetupFromProjectResponse(
            GetCollectionCustomFieldSetupFromProjectResponse response,
            GetCollectionCustomFieldSetupResult result)
        {
            response.CustomFieldSetups
                .Zip(result.CustomFieldSetups)
                .Should().AllSatisfy(x => AssertCustomFieldSetupResponse(x.First, x.Second));
        }

        private static void AssertNumberCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.NumberCustomFieldSetupResponse response,
            NumberCustomFieldSetupAggregate numberCustomFieldSetup)
        {
            if (numberCustomFieldSetup.DefaultNumber is null)
                response.DefaultNumber.Should().BeNull();
            else
                response.DefaultNumber.Should().Be(numberCustomFieldSetup.DefaultNumber.Value);
        }

        private static void AssertSingleSelectCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.SingleSelectCustomFieldSetupResponse response,
            SingleSelectCustomFieldSetupAggregate singleSelectCustomFieldSetup)
        {
            response.Options
                .Zip(singleSelectCustomFieldSetup.Options)
                .Should().AllSatisfy(x => AssertSingleSelectOptionResponse(x.First, x.Second));

            if (singleSelectCustomFieldSetup.DefaultOption is null)
                response.DefaultOption.Should().BeNull();
            else
                AssertSingleSelectOptionResponse(response.DefaultOption, singleSelectCustomFieldSetup.DefaultOption);
        }

        private static void AssertTextCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.TextCustomFieldSetupResponse response,
            TextCustomFieldSetupAggregate textCustomFieldSetup)
        {
            if (textCustomFieldSetup.DefaultText is null)
                response.DefaultText.Should().BeNull();
            else
                response.DefaultText.Should().Be(textCustomFieldSetup.DefaultText.Value);
        }

        private static void AssertSingleSelectOptionResponse(
            GetCustomFieldSetupResponse.SingleSelectCustomFieldSetupResponse.SingleSelectOptionResponse? response,
            SingleSelectOption option)
        {
            response.Should().NotBeNull();
            response!.Id.Should().Be(option.Id);
            response.Value.Should().Be(option.Value);
            response.Color.Should().Be(option.Color);
        }

        private static void AssertCustomFieldSetupResponse(
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
            GetCollectionCustomFieldSetupFromProjectResponse.SingleSelectCustomFieldSetupResponse.
                SingleSelectOptionResponse response,
            SingleSelectOption singleSelectOption)
        {
            response.Id.Should().Be(singleSelectOption.Id);
            response.Value.Should().Be(singleSelectOption.Value);
            response.Color.Should().Be(singleSelectOption.Color);
        }

        private static void AssertSingleSelectOptionCommand(
            CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand
                singleSelectOptionCommand,
            CreateCustomFieldSetupOnProjectRequest.CreateSingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest
                singleSelectOptionRequest)
        {
            singleSelectOptionCommand.Id.Should().Be(singleSelectOptionRequest.Id);
            singleSelectOptionCommand.Value.Should().Be(singleSelectOptionRequest.Value);
            singleSelectOptionCommand.Color.Should().Be(singleSelectOptionRequest.Color);
        }
    }
}