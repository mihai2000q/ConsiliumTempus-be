using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Entities;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class CustomFieldSetup
    {
        public static void AssertGetCollectionFromProjectResponse(
            GetCollectionCustomFieldSetupFromProjectResponse response,
            IEnumerable<CustomFieldSetupAggregate> customFieldSetups)
        {
            response.CustomFieldSetups
                .OrderBy(c => c.Id)
                .Zip(customFieldSetups.OrderBy(c => c.Id.Value))
                .Should().AllSatisfy(x => AssertCustomFieldSetup(x.First, x.Second));
        }

        internal static void AssertCreateOnProject(
            CreateCustomFieldSetupOnProjectRequest request,
            CustomFieldSetupAggregate customFieldSetup,
            UserAggregate user,
            ProjectAggregate project)
        {
            customFieldSetup.Id.Value.Should().NotBeEmpty();
            customFieldSetup.Name.Value.Should().Be(request.Name);
            customFieldSetup.Description.Value.Should().Be(request.Description);
            customFieldSetup.Audit.ShouldBeCreated(user);
            customFieldSetup.Workspace.Should().BeNull();
            customFieldSetup.Project.Should().Be(project);

            var customFieldType = Enum.Parse<CustomFieldType>(request.Type);
            switch (customFieldType)
            {
                case CustomFieldType.Number:
                    AssertNumberCustomFieldSetup(customFieldSetup, request);
                    break;
                case CustomFieldType.SingleSelect:
                    AssertSingleSelectCustomFieldSetup(customFieldSetup, request);
                    break;
                case CustomFieldType.Text:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request));
            }
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
            GetCollectionCustomFieldSetupFromProjectResponse.SingleSelectCustomFieldSetupResponse.
                SingleSelectOptionResponse response,
            SingleSelectOption singleSelectOption)
        {
            response.Id.Should().Be(singleSelectOption.Id);
            response.Value.Should().Be(singleSelectOption.Value);
            response.Color.Should().Be(singleSelectOption.Color);
        }

        private static void AssertNumberCustomFieldSetup(
            CustomFieldSetupAggregate customFieldSetup,
            CreateCustomFieldSetupOnProjectRequest request)
        {
            customFieldSetup.Should().BeOfType<NumberCustomFieldSetupAggregate>();
            var setup = (NumberCustomFieldSetupAggregate)customFieldSetup;
            setup.Settings.CurrencyCode.Should().Be(request.NumberSettings!.CurrencyCode);
            setup.Settings.Decimals.Should().Be((short)request.NumberSettings.Decimals);
            setup.Settings.Rounding.Should().Be(request.NumberSettings.Rounding);
        }

        private static void AssertSingleSelectCustomFieldSetup(
            CustomFieldSetupAggregate customFieldSetup,
            CreateCustomFieldSetupOnProjectRequest request)
        {
            customFieldSetup.Should().BeOfType<SingleSelectCustomFieldSetupAggregate>();
            var setup = (SingleSelectCustomFieldSetupAggregate)customFieldSetup;
            var index = 0;
            setup.Options.Zip(request.SingleSelectOptions!)
                .Should().AllSatisfy(x => AssertSingleSelectOption(x.First, x.Second, index++));
        }

        private static void AssertSingleSelectOption(
            SingleSelectOption singleSelectOption,
            CreateCustomFieldSetupOnProjectRequest.SingleSelectOptionRequest singleSelectOptionRequest,
            int index)
        {
            singleSelectOption.Value.Should().Be(singleSelectOptionRequest.Value);
            singleSelectOption.Color.Should().Be(singleSelectOptionRequest.Color);
            singleSelectOption.CustomOrderPosition.Should().Be(index);
        }
    }
}