using ConsiliumTempus.Domain.Common.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Interfaces;

public interface IAudit
{
    Audit Audit { get; }
}