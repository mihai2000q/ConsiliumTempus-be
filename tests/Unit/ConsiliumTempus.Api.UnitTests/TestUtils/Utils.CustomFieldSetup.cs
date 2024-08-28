using ConsiliumTempus.Api.Contracts.CustomFieldSetup.AddToProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Create;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.MakeGlobal;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.RemoveFromProject;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Update;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.MakeGlobal;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;
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
        private static readonly Dictionary<Type, CustomFieldType> SetupTypeToCustomFieldType = new()
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
            AssertCreateDateCustomFieldSetupCommand(command.DateCustomFieldSetup, request.DateCustomFieldSetup);
            AssertCreateDateTimeCustomFieldSetupCommand(command.DateTimeCustomFieldSetup, 
                request.DateTimeCustomFieldSetup);
            AssertCreateDurationCustomFieldSetupCommand(command.DurationCustomFieldSetup, 
                request.DurationCustomFieldSetup);
            AssertCreateMultiSelectCustomFieldSetupCommand(command.MultiSelectCustomFieldSetup,
                request.MultiSelectCustomFieldSetup);
            AssertCreateNumberCustomFieldSetupCommand(command.NumberCustomFieldSetup, request.NumberCustomFieldSetup);
            AssertCreateSingleSelectCustomFieldSetupCommand(command.SingleSelectCustomFieldSetup,
                request.SingleSelectCustomFieldSetup);
            AssertCreateTextCustomFieldSetupCommand(command.TextCustomFieldSetup, request.TextCustomFieldSetup);
            AssertCreateTimeCustomFieldSetupCommand(command.TimeCustomFieldSetup, request.TimeCustomFieldSetup);

            return true;
        }

        public static bool AssertAddCustomFieldSetupToProjectCommand(
            AddCustomFieldSetupToProjectCommand command,
            AddCustomFieldSetupToProjectRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.ProjectId.Should().Be(request.ProjectId);

            return true;
        }

        public static bool AssertUpdateCustomFieldSetupCommand(
            UpdateCustomFieldSetupCommand command,
            UpdateCustomFieldSetupRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            command.Type.Should().Be(request.Type);
            AssertUpdateNumberCustomFieldSetupCommand(command.NumberCustomFieldSetup, request.NumberCustomFieldSetup);
            AssertUpdateSingleSelectCustomFieldSetupCommand(command.SingleSelectCustomFieldSetup,
                request.SingleSelectCustomFieldSetup);
            AssertUpdateTextCustomFieldSetupCommand(command.TextCustomFieldSetup, request.TextCustomFieldSetup);

            return true;
        }

        public static bool AssertMakeCustomFieldSetupGlobalCommand(
            MakeCustomFieldSetupGlobalCommand command,
            MakeCustomFieldSetupGlobalRequest request)
        {
            command.Id.Should().Be(request.Id);

            return true;
        }

        public static bool AssertDeleteCustomFieldSetupCommand(
            DeleteCustomFieldSetupCommand command,
            DeleteCustomFieldSetupRequest request)
        {
            command.Id.Should().Be(request.Id);

            return true;
        }

        public static bool AssertRemoveCustomFieldSetupFromProjectCommand(
            RemoveCustomFieldSetupFromProjectCommand command,
            RemoveCustomFieldSetupFromProjectRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.ProjectId.Should().Be(request.ProjectId);

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
                case GetCustomFieldSetupResponse.DateCustomFieldSetupResponse dateSetup:
                    result.CustomFieldSetup.Should().BeOfType<DateCustomFieldSetupAggregate>();
                    AssertDateCustomFieldSetupResponse(dateSetup,
                        (DateCustomFieldSetupAggregate)result.CustomFieldSetup);
                    break;

                case GetCustomFieldSetupResponse.DateTimeCustomFieldSetupResponse dateTimeSetup:
                    result.CustomFieldSetup.Should().BeOfType<DateTimeCustomFieldSetupAggregate>();
                    AssertDateTimeCustomFieldSetupResponse(dateTimeSetup,
                        (DateTimeCustomFieldSetupAggregate)result.CustomFieldSetup);
                    break;

                case GetCustomFieldSetupResponse.DurationCustomFieldSetupResponse durationSetup:
                    result.CustomFieldSetup.Should().BeOfType<DurationCustomFieldSetupAggregate>();
                    AssertDurationCustomFieldSetupResponse(durationSetup,
                        (DurationCustomFieldSetupAggregate)result.CustomFieldSetup);
                    break;

                case GetCustomFieldSetupResponse.MultiSelectCustomFieldSetupResponse multiSelectSetup:
                    result.CustomFieldSetup.Should().BeOfType<MultiSelectCustomFieldSetupAggregate>();
                    AssertMultiSelectCustomFieldSetupResponse(multiSelectSetup,
                        (MultiSelectCustomFieldSetupAggregate)result.CustomFieldSetup);
                    break;

                case GetCustomFieldSetupResponse.NumberCustomFieldSetupResponse numberSetup:
                    result.CustomFieldSetup.Should().BeOfType<NumberCustomFieldSetupAggregate>();
                    AssertNumberCustomFieldSetupResponse(numberSetup,
                        (NumberCustomFieldSetupAggregate)result.CustomFieldSetup);
                    break;

                case GetCustomFieldSetupResponse.PeopleCustomFieldSetupResponse peopleSetup:
                    result.CustomFieldSetup.Should().BeOfType<PeopleCustomFieldSetupAggregate>();
                    AssertPeopleCustomFieldSetupResponse(peopleSetup,
                        (PeopleCustomFieldSetupAggregate)result.CustomFieldSetup);
                    break;

                case GetCustomFieldSetupResponse.SingleSelectCustomFieldSetupResponse singleSelectSetup:
                    result.CustomFieldSetup.Should().BeOfType<SingleSelectCustomFieldSetupAggregate>();
                    AssertSingleSelectCustomFieldSetupResponse(singleSelectSetup,
                        (SingleSelectCustomFieldSetupAggregate)result.CustomFieldSetup);
                    break;

                case GetCustomFieldSetupResponse.TextCustomFieldSetupResponse textSetup:
                    result.CustomFieldSetup.Should().BeOfType<TextCustomFieldSetupAggregate>();
                    AssertTextCustomFieldSetupResponse(textSetup,
                        (TextCustomFieldSetupAggregate)result.CustomFieldSetup);
                    break;

                case GetCustomFieldSetupResponse.TimeCustomFieldSetupResponse timeSetup:
                    result.CustomFieldSetup.Should().BeOfType<TimeCustomFieldSetupAggregate>();
                    AssertTimeCustomFieldSetupResponse(timeSetup,
                        (TimeCustomFieldSetupAggregate)result.CustomFieldSetup);
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

        private static void AssertDateCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.DateCustomFieldSetupResponse response,
            DateCustomFieldSetupAggregate setup)
        {
            response.Type.Should().Be(CustomFieldType.Date);
            response.DefaultDate.Should().Be(setup.DefaultDate);
        }

        private static void AssertDateTimeCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.DateTimeCustomFieldSetupResponse response,
            DateTimeCustomFieldSetupAggregate setup)
        {
            response.Type.Should().Be(CustomFieldType.DateTime);
            response.DefaultDateTime.Should().Be(setup.DefaultDateTime);
        }

        private static void AssertDurationCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.DurationCustomFieldSetupResponse response,
            DurationCustomFieldSetupAggregate setup)
        {
            response.Type.Should().Be(CustomFieldType.Duration);
            response.DefaultDuration.Should().Be(setup.DefaultDuration);
        }

        private static void AssertMultiSelectCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.MultiSelectCustomFieldSetupResponse response,
            MultiSelectCustomFieldSetupAggregate setup)
        {
            response.Type.Should().Be(CustomFieldType.SingleSelect);
            response.Options
                .Zip(setup.Options)
                .Should().AllSatisfy(x => AssertMultiSelectOptionResponse(x.First, x.Second));
        }

        private static void AssertNumberCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.NumberCustomFieldSetupResponse response,
            NumberCustomFieldSetupAggregate setup)
        {
            response.Type.Should().Be(CustomFieldType.Number);
            response.Settings.CurrencyCode.Should().Be(setup.Settings.CurrencyCode);
            response.Settings.Decimals.Should().Be(setup.Settings.Decimals);
            response.Settings.Rounding.Should().Be(setup.Settings.Rounding);
            if (setup.DefaultNumber is null)
                response.DefaultNumber.Should().BeNull();
            else
                response.DefaultNumber.Should().Be(setup.DefaultNumber.Value);
        }

        private static void AssertPeopleCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.PeopleCustomFieldSetupResponse response,
            PeopleCustomFieldSetupAggregate setup)
        {
            response.Type.Should().Be(CustomFieldType.People);
            setup.Should().NotBeNull(); // obviously true
        }

        private static void AssertSingleSelectCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.SingleSelectCustomFieldSetupResponse response,
            SingleSelectCustomFieldSetupAggregate setup)
        {
            response.Type.Should().Be(CustomFieldType.SingleSelect);
            response.Options
                .Zip(setup.Options)
                .Should().AllSatisfy(x => AssertSingleSelectOptionResponse(x.First, x.Second));

            if (setup.DefaultOption is null)
                response.DefaultOption.Should().BeNull();
            else
                AssertSingleSelectOptionResponse(response.DefaultOption, setup.DefaultOption);
        }

        private static void AssertTextCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.TextCustomFieldSetupResponse response,
            TextCustomFieldSetupAggregate setup)
        {
            response.Type.Should().Be(CustomFieldType.Text);
            if (setup.DefaultText is null)
                response.DefaultText.Should().BeNull();
            else
                response.DefaultText.Should().Be(setup.DefaultText.Value);
        }

        private static void AssertTimeCustomFieldSetupResponse(
            GetCustomFieldSetupResponse.TimeCustomFieldSetupResponse response,
            TimeCustomFieldSetupAggregate setup)
        {
            response.Type.Should().Be(CustomFieldType.Time);
            response.DefaultTime.Should().Be(setup.DefaultTime);
        }

        private static void AssertMultiSelectOptionResponse(
            GetCustomFieldSetupResponse.MultiSelectCustomFieldSetupResponse.MultiSelectOptionResponse response,
            MultiSelectOption option)
        {
            response.Id.Should().Be(option.Id);
            response.Value.Should().Be(option.Value);
            response.Color.Should().Be(option.Color);
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
            response.Type.Should().Be(SetupTypeToCustomFieldType[customFieldSetup.GetType()]);
        }

        private static void AssertCustomFieldSetupResponse(
            GetCollectionCustomFieldSetupFromProjectResponse.CustomFieldSetupResponse response,
            CustomFieldSetupAggregate customFieldSetup)
        {
            response.Id.Should().Be(customFieldSetup.Id.Value);
            response.Name.Should().Be(customFieldSetup.Name.Value);
            response.Description.Should().Be(customFieldSetup.Description.Value);
            response.Type.Should().Be(SetupTypeToCustomFieldType[customFieldSetup.GetType()]);
        }
        
        private static void AssertCreateDateCustomFieldSetupCommand(
            CreateCustomFieldSetupCommand.DateCustomFieldSetupCommand? command,
            CreateCustomFieldSetupRequest.DateCustomFieldSetupRequest? request)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command.Should().NotBeNull();
            command!.DefaultDate.Should().Be(request.DefaultDate);
        }
        
        private static void AssertCreateDateTimeCustomFieldSetupCommand(
            CreateCustomFieldSetupCommand.DateTimeCustomFieldSetupCommand? command,
            CreateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest? request)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command.Should().NotBeNull();
            command!.DefaultDateTime.Should().Be(request.DefaultDateTime);
        }
        
        private static void AssertCreateDurationCustomFieldSetupCommand(
            CreateCustomFieldSetupCommand.DurationCustomFieldSetupCommand? command,
            CreateCustomFieldSetupRequest.DurationCustomFieldSetupRequest? request)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command.Should().NotBeNull();
            command!.DefaultDuration.Should().Be(request.DefaultDuration);
        }
        
        private static void AssertCreateMultiSelectCustomFieldSetupCommand(
            CreateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand? command,
            CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest? request)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command.Should().NotBeNull();
            command!.Options
                .Zip(request.Options)
                .Should().AllSatisfy(x => AssertCreateMultiSelectOptionCommand(x.First, x.Second));
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

            command.Should().NotBeNull();
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

            command.Should().NotBeNull();
            command!.Options
                .Zip(request.Options)
                .Should().AllSatisfy(x => AssertCreateSingleSelectOptionCommand(x.First, x.Second));
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

            command.Should().NotBeNull();
            command!.DefaultText.Should().Be(request.DefaultText);
        }
        
        private static void AssertCreateTimeCustomFieldSetupCommand(
            CreateCustomFieldSetupCommand.TimeCustomFieldSetupCommand? command,
            CreateCustomFieldSetupRequest.TimeCustomFieldSetupRequest? request)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command.Should().NotBeNull();
            command!.DefaultTime.Should().Be(request.DefaultTime);
        }
        
        private static void AssertCreateMultiSelectOptionCommand(
            CreateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand command,
            CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest.MultiSelectOptionRequest request)
        {
            command.Value.Should().Be(request.Value);
            command.Color.Should().Be(request.Color);
        }

        private static void AssertCreateSingleSelectOptionCommand(
            CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand command,
            CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Value.Should().Be(request.Value);
            command.Color.Should().Be(request.Color);
        }

        private static void AssertUpdateNumberCustomFieldSetupCommand(
            UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand? command,
            UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? request)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command.Should().NotBeNull();
            command!.Settings.CurrencyCode.Should().Be(request.Settings.CurrencyCode);
            command.Settings.Decimals.Should().Be(request.Settings.Decimals);
            command.Settings.Rounding.Should().Be(request.Settings.Rounding);
            command.DefaultNumber.Should().Be(request.DefaultNumber);
        }

        private static void AssertUpdateSingleSelectCustomFieldSetupCommand(
            UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand? command,
            UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? request)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command.Should().NotBeNull();
            command!.DefaultOptionId.Should().Be(request.DefaultOptionId);
            command.Operation.ToString().Should().Be(request.Operation?.ToString());
            AssertUpdateSingleSelectOptionCommand(command.NewOption, request.NewOption);
            command.OptionId.Should().Be(request.OptionId);
            command.OverOptionId.Should().Be(request.OverOptionId);
        }

        private static void AssertUpdateTextCustomFieldSetupCommand(
            UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand? command,
            UpdateCustomFieldSetupRequest.TextCustomFieldSetupRequest? request)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command.Should().NotBeNull();
            command!.DefaultText.Should().Be(request.DefaultText);
        }

        private static void AssertUpdateSingleSelectOptionCommand(
            UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand? command,
            UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest? request)
        {
            if (request is null)
            {
                command.Should().BeNull();
                return;
            }

            command.Should().NotBeNull();
            command!.Color.Should().Be(request.Color);
            command.Value.Should().Be(request.Value);
        }
    }
}