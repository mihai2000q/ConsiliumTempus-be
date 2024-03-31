using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Http;

internal static class HttpRequestReader
{
    internal static string? GetStringIdFromRoute(HttpRequest request, string property = "id")
    {
        var id = (string?)request.RouteValues[property];

        return !string.IsNullOrEmpty(id) ? id : null;
    }

    internal static Task<string?> GetStringIdFromBody(HttpRequest request)
    {
        return GetPropertyFromBody(request, "id");
    }

    internal static async Task<string?> GetPropertyFromBody(HttpRequest request, string property)
    {
        request.EnableBuffering();
        var body = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(request.Body);
        request.Body.Position = 0;

        if (body is null) return null;
        if (!body.TryGetValue(property, out var jsonElement)) return null;
        if (jsonElement.ValueKind != JsonValueKind.String) return null;
        var id = jsonElement.GetString();
        return !string.IsNullOrEmpty(id) ? id : null;
    }
}