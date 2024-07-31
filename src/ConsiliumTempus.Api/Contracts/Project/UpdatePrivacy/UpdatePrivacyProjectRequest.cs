namespace ConsiliumTempus.Api.Contracts.Project.UpdatePrivacy;

public sealed record UpdatePrivacyProjectRequest(
    Guid Id,
    bool IsPrivate);