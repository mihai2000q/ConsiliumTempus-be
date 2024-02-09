namespace ConsiliumTempus.Domain.Common.Interfaces;

public interface ITimestamps
{
    DateTime CreatedDateTime { get; }
    DateTime UpdatedDateTime { get; }
}