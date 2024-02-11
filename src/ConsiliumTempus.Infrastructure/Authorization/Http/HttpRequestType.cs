using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Infrastructure.Authorization.Http;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class HttpRequestType
{
    internal const string GET = "GET";
    internal const string POST = "POST";
    internal const string PUT = "PUT";
    internal const string DELETE = "DELETE";
}