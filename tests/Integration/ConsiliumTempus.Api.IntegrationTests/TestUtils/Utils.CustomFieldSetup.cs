using ConsiliumTempus.Api.Contracts.CustomFieldSetup.AddToProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Create;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.RemoveFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.UpdateWorkspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class CustomFieldSetup
    {
        public static void AssertAddToProject(
            AddCustomFieldSetupToProjectRequest request,
            CustomFieldSetupAggregate customFieldSetup,
            UserAggregate user)
        {
            customFieldSetup.Projects.Should().ContainSingle(p => p.Id.Value == request.ProjectId);
            customFieldSetup.Audit.ShouldBeUpdated(user);

            customFieldSetup.Workspace!.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            var project = customFieldSetup.Projects.Single(p => p.Id.Value == request.ProjectId);
            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertCreation(
            CreateCustomFieldSetupRequest request,
            CustomFieldSetupAggregate customFieldSetup,
            UserAggregate user,
            List<ProjectTaskAggregate> tasks)
        {
            customFieldSetup.Id.Value.Should().NotBeEmpty();
            customFieldSetup.Name.Value.Should().Be(request.Name);
            customFieldSetup.Description.Value.Should().Be(request.Description);
            customFieldSetup.Audit.ShouldBeCreated(user);
            switch (request)
            {
                case CreateCustomFieldSetupOnWorkspaceRequest workspaceRequest:
                    customFieldSetup.Projects.Should().BeEmpty();
                    customFieldSetup.Workspace.Should().NotBeNull();
                    customFieldSetup.Workspace!.Id.Value.Should().Be(workspaceRequest.WorkspaceId);
                    customFieldSetup.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
                    break;

                case CreateCustomFieldSetupOnProjectRequest projectRequest:
                    customFieldSetup.Workspace.Should().BeNull();
                    customFieldSetup.Projects.Should().HaveCount(1);
                    customFieldSetup.Projects[0].Id.Value.Should().Be(projectRequest.ProjectId);
                    customFieldSetup.Projects[0].LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
                    customFieldSetup.Projects[0].Workspace.LastActivity.Should()
                        .BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
                    break;
            }

            switch (request.Type)
            {
                case CustomFieldType.Number:
                    AssertNumberCustomFieldSetup(customFieldSetup, request);
                    break;
                case CustomFieldType.SingleSelect:
                    AssertSingleSelectCustomFieldSetup(customFieldSetup, request);
                    break;
                case CustomFieldType.Text:
                    AssertTextCustomFieldSetup(customFieldSetup, request);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request));
            }

            tasks.Should().AllSatisfy(task =>
            {
                task.CustomFields.Should().HaveCount(1);
                var customField = task.CustomFields[0];
                switch (request.Type)
                {
                    case CustomFieldType.Number:
                        customField.Should().BeOfType<NumberCustomField>();
                        customFieldSetup.Should().BeOfType<NumberCustomFieldSetupAggregate>();
                        ((NumberCustomField)customField).Setup.Should().Be(customFieldSetup);
                        ((NumberCustomField)customField).Number
                            .Should().Be(((NumberCustomFieldSetupAggregate)customFieldSetup).DefaultNumber);
                        break;
                    case CustomFieldType.SingleSelect:
                        customField.Should().BeOfType<SingleSelectCustomField>();
                        customFieldSetup.Should().BeOfType<SingleSelectCustomFieldSetupAggregate>();
                        ((SingleSelectCustomField)customField).Setup.Should().Be(customFieldSetup);
                        ((SingleSelectCustomField)customField).Option
                            .Should().Be(((SingleSelectCustomFieldSetupAggregate)customFieldSetup).DefaultOption);
                        break;
                    case CustomFieldType.Text:
                        customField.Should().BeOfType<TextCustomField>();
                        customFieldSetup.Should().BeOfType<TextCustomFieldSetupAggregate>();
                        ((TextCustomField)customField).Setup.Should().Be(customFieldSetup);
                        ((TextCustomField)customField).Text
                            .Should().Be(((TextCustomFieldSetupAggregate)customFieldSetup).DefaultText);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(request));
                }
            });
        }

        public static void AssertRemoveFromProject(
            RemoveCustomFieldSetupFromProjectRequest request,
            CustomFieldSetupAggregate customFieldSetup,
            ProjectAggregate project,
            UserAggregate user)
        {
            customFieldSetup.Projects.Should().NotContain(p => p.Id.Value == request.ProjectId);
            customFieldSetup.Audit.ShouldBeUpdated(user);

            customFieldSetup.Workspace!.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        public static void AssertUpdateWorkspace(
            UpdateWorkspaceCustomFieldSetupRequest request,
            CustomFieldSetupAggregate customFieldSetup,
            UserAggregate user)
        {
            customFieldSetup.Workspace.Should().NotBeNull();
            customFieldSetup.Workspace!.Id.Value.Should().Be(request.WorkspaceId);
            customFieldSetup.Audit.ShouldBeUpdated(user);
        }

        public static void AssertGetResponse(
            GetCustomFieldSetupResponse response,
            CustomFieldSetupAggregate customFieldSetup)
        {
            response.CustomFieldSetup.Id.Should().Be(customFieldSetup.Id.Value);
            response.CustomFieldSetup.Name.Should().Be(customFieldSetup.Name.Value);
            response.CustomFieldSetup.Description.Should().Be(customFieldSetup.Description.Value);
            switch (response.CustomFieldSetup)
            {
                case GetCustomFieldSetupResponse.NumberCustomFieldSetupResponse numberSetup:
                    AssertNumberCustomFieldSetupResponse(numberSetup,
                        (NumberCustomFieldSetupAggregate)customFieldSetup);
                    break;
                case GetCustomFieldSetupResponse.SingleSelectCustomFieldSetupResponse singleSelectSetup:
                    AssertSingleSelectCustomFieldSetupResponse(singleSelectSetup,
                        (SingleSelectCustomFieldSetupAggregate)customFieldSetup);
                    break;
                case GetCustomFieldSetupResponse.TextCustomFieldSetupResponse textSetup:
                    AssertTextCustomFieldSetupResponse(textSetup,
                        (TextCustomFieldSetupAggregate)customFieldSetup);
                    break;
            }
        }

        public static void AssertGetCollectionFromWorkspaceResponse(
            GetCollectionCustomFieldSetupFromWorkspaceResponse response,
            IEnumerable<CustomFieldSetupAggregate> customFieldSetups)
        {
            response.CustomFieldSetups
                .Zip(customFieldSetups.OrderBy(c => c.Audit.CreatedDateTime))
                .Should().AllSatisfy(x => AssertCustomFieldSetup(x.First, x.Second));
        }

        public static void AssertGetCollectionFromProjectResponse(
            GetCollectionCustomFieldSetupFromProjectResponse response,
            IEnumerable<CustomFieldSetupAggregate> customFieldSetups)
        {
            response.CustomFieldSetups
                .Zip(customFieldSetups.OrderBy(c => c.Audit.CreatedDateTime))
                .Should().AllSatisfy(x => AssertCustomFieldSetup(x.First, x.Second));
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

        private static void AssertCustomFieldSetup(
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

        private static void AssertCustomFieldSetup(
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

        private static void AssertNumberCustomFieldSetup(
            CustomFieldSetupAggregate customFieldSetup,
            CreateCustomFieldSetupRequest request)
        {
            customFieldSetup.Should().BeOfType<NumberCustomFieldSetupAggregate>();
            var setup = (NumberCustomFieldSetupAggregate)customFieldSetup;
            setup.Settings.CurrencyCode.Should().Be(request.NumberCustomFieldSetup!.Settings.CurrencyCode);
            setup.Settings.Decimals.Should().Be((short)request.NumberCustomFieldSetup.Settings.Decimals);
            setup.Settings.Rounding.Should().Be(request.NumberCustomFieldSetup.Settings.Rounding);
        }

        private static void AssertSingleSelectCustomFieldSetup(
            CustomFieldSetupAggregate customFieldSetup,
            CreateCustomFieldSetupRequest request)
        {
            customFieldSetup.Should().BeOfType<SingleSelectCustomFieldSetupAggregate>();
            var setup = (SingleSelectCustomFieldSetupAggregate)customFieldSetup;
            var index = 0;
            setup.Options
                .Zip(request.SingleSelectCustomFieldSetup!.Options)
                .Should().AllSatisfy(x => AssertSingleSelectOption(x.First, x.Second, index++));
            if (request.SingleSelectCustomFieldSetup.DefaultOptionId is null)
                setup.DefaultOption.Should().BeNull();
            else
            {
                var optionIndex = request.SingleSelectCustomFieldSetup.Options
                    .FindIndex(o => o.Id == request.SingleSelectCustomFieldSetup.DefaultOptionId);
                setup.DefaultOption.Should().Be(setup.Options[optionIndex]);
                setup.DefaultOptionId.Should().Be(setup.Options[optionIndex].Id);
            }
        }

        private static void AssertTextCustomFieldSetup(
            CustomFieldSetupAggregate customFieldSetup,
            CreateCustomFieldSetupRequest request)
        {
            customFieldSetup.Should().BeOfType<TextCustomFieldSetupAggregate>();
            var setup = (TextCustomFieldSetupAggregate)customFieldSetup;
            if (request.TextCustomFieldSetup!.DefaultText is null)
                setup.DefaultText.Should().BeNull();
            else
                setup.DefaultText!.Value.Should().Be(request.TextCustomFieldSetup.DefaultText);
        }

        private static void AssertSingleSelectOption(
            SingleSelectOption singleSelectOption,
            CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest
                singleSelectOptionRequest,
            int customOrderPosition)
        {
            singleSelectOption.Value.Should().Be(singleSelectOptionRequest.Value);
            singleSelectOption.Color.Should().Be(singleSelectOptionRequest.Color);
            singleSelectOption.CustomOrderPosition.Value.Should().Be(customOrderPosition);
        }
    }
}