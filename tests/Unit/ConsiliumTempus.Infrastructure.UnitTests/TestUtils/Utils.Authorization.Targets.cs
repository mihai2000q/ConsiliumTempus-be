using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ConsiliumTempus.Infrastructure.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static partial class Authorization
    {
        internal static class Targets
        {
            internal static HttpRequest CreateRequest(Controller controller, Action action, Method method)
            {
                return CreateRequest(controller.ToString(), action.ToString(), method.ToString());
            }

            /// <summary>
            /// For the requests that do not go to an endpoint of a known controller, action and HTTP method
            /// </summary>
            internal static HttpRequest CreateRequest(string? controller, string? action, string method)
            {
                var context = new DefaultHttpContext
                {
                    Request =
                    {
                        Method = method,
                        RouteValues = new RouteValueDictionary { ["controller"] = controller, ["action"] = action }
                    }
                };
                return context.Request;
            }
        }
    }
}
