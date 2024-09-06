using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Relations;

public sealed class CustomFieldMultiSelectHasOption : Entity<(CustomFieldId, Guid)>
{
    private CustomFieldMultiSelectHasOption() {}

    // public override (CustomFieldId, Guid)? Id => new(MultiSelectCustomFieldId, OptionsId);
    // public CustomFieldId MultiSelectCustomFieldId { get; init; } = null!;
    public Guid OptionsId { get; init; }
}