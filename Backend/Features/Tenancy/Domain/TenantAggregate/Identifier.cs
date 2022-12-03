namespace Backend.Features.Tenancy.Domain.TenantAggregate;

public class Identifier
{
    private Identifier(string value)
    {
        Value = value;
    }

    private Identifier()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public string Value { get; private set; } = null!;

    public static Identifier CreateInstance(string identifier)
    {
        return new Identifier(identifier);
    }
}