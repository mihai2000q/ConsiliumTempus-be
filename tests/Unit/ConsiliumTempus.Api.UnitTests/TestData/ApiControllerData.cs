using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace ConsiliumTempus.Api.UnitTests.TestData;

internal static class ApiControllerData
{
    internal class GetSingleErrors : TheoryData<Error, int>
    {
        public GetSingleErrors()
        {
            Add(Error.Unauthorized("Error.Unauthorized", "Unauthorized"), StatusCodes.Status401Unauthorized);
            Add(Error.Forbidden("Error.Forbidden", "Forbidden"), StatusCodes.Status403Forbidden);
            Add(Error.NotFound("Error.NotFound", "Not found"), StatusCodes.Status404NotFound);
            Add(Error.Conflict("Error.Conflict", "Conflict"), StatusCodes.Status409Conflict);
            Add(Error.Failure("Error.Failure", "Failure"), StatusCodes.Status500InternalServerError);
            Add(Error.Unexpected("Error.Unexpected", "Unexpected"), StatusCodes.Status500InternalServerError);
        }
    }

    internal class GetMixedErrors : TheoryData<List<Error>, int>
    {
        public GetMixedErrors()
        {
            var notFound = Error.NotFound("Error.NotFound", "Not found");
            var validation = Error.Validation("Error.Validation", "Validation");
            var conflict = Error.Conflict("Error.Conflict", "Conflict");

            Add([notFound, validation], StatusCodes.Status404NotFound);
            Add([validation, notFound], StatusCodes.Status400BadRequest);
            Add([conflict, notFound, validation], StatusCodes.Status409Conflict);
        }
    }
}
