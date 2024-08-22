using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Create;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.UpdateWorkspace;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.UpdateWorkspace;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;
using ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
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
            GetCollectionCustomFieldSetupFromWorkspaceRequest request)
        {
            query.WorkspaceId.Should().Be(request.WorkspaceId);
            query.ProjectId.Should().BeNull();

            return true;
        }

        public static bool AssertGetCollectionCustomFieldSetupQuery(
            GetCollectionCustomFieldSetupQuery query,
            GetCollectionCustomFieldSetupFromProjectRequest request)
        {
            query.WorkspaceId.Should().BeNull();
            query.ProjectId.Should().Be(request.ProjectId);

            return true;
        }

        public static bool AssertCreateCustomFieldSetupCommand(
            CreateCustomFieldSetupCommand command,
            CreateCustomFieldSetupRequest request)
        {
            switch (request)
            {
                case CreateCustomFieldSetupOnProjectRequest pr:
                    command.WorkspaceId.Should().BeNull();
                    command.ProjectId.Should().Be(pr.ProjectId);
                    break;
                case CreateCustomFieldSetupOnWorkspaceRequest wr:
                    command.ProjectId.Should().BeNull();
                    command.WorkspaceId.Should().Be(wr.WorkspaceId);
                    break;
            }

            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            command.Type.Should().Be(request.Type);
            AssertCreateNumberCustomFieldSetupCommand(command.NumberCustomFieldSetup, request.NumberCustomFieldSetup);
            AssertCreateSingleSelectCustomFieldSetupCommand(command.SingleSelectCustomFieldSetup, request.SingleSelectCustomFieldSetup);
            AssertCreateTextCustomFieldSetupCommand(command.TextCustomFieldSetup, request.TextCustomFieldSetup);

            return true;
        }

        public static bool AssertUpdateWorkspaceCustomFieldSetupCommand(
            UpdateWorkspaceCustomFieldSetupCommand command,
            UpdateWorkspaceCustomFieldSetupRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.WorkspaceId.Should().Be(request.WorkspaceId);

            return true;
        }

        public static bool AssertDeleteCustomFieldSetupCommand(
            DeleteCustomFieldSetupCommand command,
            DeleteCustomFieldSetupRequest request)
        {
            command.Id.Should().Be(request.Id);

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

        public static void AssertGetCollectionFromWorkspaceResponse(
            GetCollectionCustomFieldSetupFromWorkspaceResponse response,
            GetCollectionCustomFieldSetupResult result)
        {
            response.CustomFieldSetups
                .Zip(result.CustomFieldSetups)
                .Should().AllSatisfy(x => AssertCustomFieldSetupResponse(x.First, x.Second));
        }

        public static void AssertGetCollectionFromProjectResponse(
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
            response.Type.Should().Be(CustomFieldType.Number);
            response.Settings.CurrencyCode.Should().Be(numberCustomFieldSetup.Settings.CurrencyCode);
            response.Settings.Decimals.Should().Be(numberCustomFieldSetup.Settings.Decimals);
            response.Settings.Rounding.Should().Be(numberCustomFieldSetup.Settings.Rounding);
            if (numberCustomFieldSetup.DefaultNumber is null)
                response.DefaultNumber.Should().BeNull();
            else
                response.DefaultNumber.Should().Be(numberCustomFieldSetup.DefaultNumber.Value);
        }

        private static void AssertSingleSelectCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.SingleSelectCustomFieldSetupResponse response,
            SingleSelectCustomFieldSetupAggregate singleSelectCustomFieldSetup)
        {
            response.Type.Should().Be(CustomFieldType.SingleSelect);
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
            response.Type.Should().Be(CustomFieldType.Text);
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
            GetCollectionCustomFieldSetupFromWorkspaceResponse.CustomFieldSetupResponse response,
            CustomFieldSetupAggregate customFieldSetup)
        {
            response.Id.Should().Be(customFieldSetup.Id.Value);
            response.Name.Should().Be(customFieldSetup.Name.Value);
            response.Description.Should().Be(customFieldSetup.Description.Value);

            var mapSetupTypeToEnumType = new Dictionary<Type, CustomFieldType>
            {
                { typeof(NumberCustomFieldSetupAggregate), CustomFieldType.Number },
                { typeof(SingleSelectCustomFieldSetupAggregate), CustomFieldType.SingleSelect },
                { typeof(TextCustomFieldSetupAggregate), CustomFieldType.Text },
            };

            response.Type.Should().Be(mapSetupTypeToEnumType[customFieldSetup.GetType()]);
        }

        private static void AssertCustomFieldSetupResponse(
            GetCollectionCustomFieldSetupFromProjectResponse.CustomFieldSetupResponse response,
            CustomFieldSetupAggregate customFieldSetup)
        {
            response.Id.Should().Be(customFieldSetup.Id.Value);
            response.Name.Should().Be(customFieldSetup.Name.Value);
            response.Description.Should().Be(customFieldSetup.Description.Value);

            var mapSetupTypeToEnumType = new Dictionary<Type, CustomFieldType>
            {
                { typeof(NumberCustomFieldSetupAggregate), CustomFieldType.Number },
                { typeof(SingleSelectCustomFieldSetupAggregate), CustomFieldType.SingleSelect },
                { typeof(TextCustomFieldSetupAggregate), CustomFieldType.Text },
            };

            response.Type.Should().Be(mapSetupTypeToEnumType[customFieldSetup.GetType()]);
        }
        
        private static void AssertCreateNumberCustomFieldSetupCommand(
            CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand? command,
            CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? request)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command!.Settings.CurrencyCode.Should().Be(request.Settings.CurrencyCode);
            command.Settings.Decimals.Should().Be(request.Settings.Decimals);
            command.Settings.Rounding.Should().Be(request.Settings.Rounding);
            command.DefaultNumber.Should().Be(request.DefaultNumber);
        }
        
        private static void AssertCreateSingleSelectCustomFieldSetupCommand(
            CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand? command,
            CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? request)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command!.Options
                .Zip(request.Options)
                .Should().AllSatisfy(x => AssertSingleSelectOptionCommand(x.First, x.Second));
            command.DefaultOptionId.Should().Be(request.DefaultOptionId);
        }
        
        private static void AssertCreateTextCustomFieldSetupCommand(
            CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand? command,
            CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest? request)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command!.DefaultText.Should().Be(request.DefaultText);
        }

        private static void AssertSingleSelectOptionCommand(
            CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand
                singleSelectOptionCommand,
            CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest
                singleSelectOptionRequest)
        {
            singleSelectOptionCommand.Id.Should().Be(singleSelectOptionRequest.Id);
            singleSelectOptionCommand.Value.Should().Be(singleSelectOptionRequest.Value);
            singleSelectOptionCommand.Color.Should().Be(singleSelectOptionRequest.Color);
        }
    }
}