using ConsiliumTempus.Domain.Common.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Interfaces;

public interface IAuthored
{
    Authorship Authorship { get; }
}
