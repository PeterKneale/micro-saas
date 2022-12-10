namespace Backend.Features.Tenancy.Domain.UserAggregate;

public class Password
{
    private Password(string? value)
    {
        Value = value;
    }

    private Password()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public string? Value { get; private set; } = null!;

    public static Password CreateInstance(string value)
    {
        return new Password(value);
    }
    
    public static Password Empty()
    {
        return new Password(null);
    }
}