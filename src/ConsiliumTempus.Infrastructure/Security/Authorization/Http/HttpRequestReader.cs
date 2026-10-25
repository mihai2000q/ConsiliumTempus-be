using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Http;

internal static class HttpRequestReader
{
    internal static string? GetStringIdFromRoute(HttpRequest request, string property = "id")
    {
        var id = request.RouteValues[property]?.ToString();
        return string.IsNullOrEmpty(id) ? null : id;
    }

    internal static string? GetStringIdFromQuery(HttpRequest request, string property = "id")
    {
        var id = request.Query[property].ToString();
        return string.IsNullOrEmpty(id) ? null : id;
    }

    internal static async Task<string?> GetStringIdFromBody(HttpRequest request, string property = "id")
    {
        request.EnableBuffering();
        try
        {
            using var body = await JsonDocument.ParseAsync(request.Body);
            if (body.RootElement.ValueKind != JsonValueKind.Object) return null;

            // the model binding matches the property names case-insensitively, so this has to do it as well,
            // otherwise the id would be missed here, but still bound afterwards
            var jsonElement = body.RootElement
                .EnumerateObject()
                .Where(p => string.Equals(p.Name, property, StringComparison.OrdinalIgnoreCase))
                .Select(p => (JsonElement?)p.Value)
                .LastOrDefault();

            if (jsonElement is not { ValueKind: JsonValueKind.String }) return null;
            var id = jsonElement.Value.GetString();
            return !string.IsNullOrEmpty(id) ? id : null;
        }
        catch (JsonException)
        {
            return null; // let the system return the validation error
        }
        finally
        {
            request.Body.Position = 0;
        }
    }
}