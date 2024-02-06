using ConsiliumTempus.Domain.User;
using ErrorOr;

namespace ConsiliumTempus.Application.Common.Security;

public interface ISecurity
{
    Task<ErrorOr<UserAggregate>> GetUserFromToken(string plainToken);
}