using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace ConsiliumTempus.Infrastructure.Authorization.Http;

internal static class HttpRequestReader
{
    internal static string? GetStringIdFromRoute(HttpRequest request)
    {
        var id = (string?)request.RouteValues["id"];

        return !string.IsNullOrEmpty(id) ? id : null;
    }

    internal static async Task<string?> GetStringIdFromBody(HttpRequest request)
    {
        request.EnableBuffering();
        var body = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(request.Body);
        request.Body.Position = 0;
        
        var id = body?["id"] ?? "";
        return !string.IsNullOrEmpty(id) ? id : null;
    }
}