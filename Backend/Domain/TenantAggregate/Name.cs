namespace Backend.Domain.TenantAggregate;

public class Name
{
    private Name(string value)
    {
        Value = value;
    }

    private Name()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public string Value { get; private set; } = null!;

    public static Name CreateInstance(string value)
    {
        return new Name(value);
    }
}