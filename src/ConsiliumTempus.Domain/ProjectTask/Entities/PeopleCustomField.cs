using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public sealed class PeopleCustomField : CustomField
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private PeopleCustomField()
    {
    }

    private PeopleCustomField(
        UserAggregate? person,
        PeopleCustomFieldSetupAggregate setup,
        CustomFieldId id,
        ProjectTaskAggregate projectTask) : base(id, projectTask)
    {
        Person = person;
        Setup = setup;
    }

    public UserAggregate? Person { get; private set; }
    public override PeopleCustomFieldSetupAggregate Setup { get; } = null!;

    public static PeopleCustomField Create(
        UserAggregate? person,
        PeopleCustomFieldSetupAggregate setup,
        ProjectTaskAggregate projectTask)
    {
        return new PeopleCustomField(
            person,
            setup,
            CustomFieldId.CreateUnique(),
            projectTask);
    }

    public void Update(UserAggregate? person)
    {
        Person = person;
    }
}