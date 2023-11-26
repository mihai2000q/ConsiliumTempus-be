namespace ConsiliumTempus.Domain.UserAggregate.ValueObjects;

public class UserId
{
    public Guid Value { get; set; }

    private UserId(Guid value)
    {
        Value = value;
    }
    
    public static UserId CreateUnique()
    {
        return new UserId(Guid.NewGuid());
    }
}