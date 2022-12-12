namespace Backend.Features.Tenancy.Domain.Common;

public class TenantIdentifier
{
    private TenantIdentifier(string value)
    {
        Value = value;
    }

    private TenantIdentifier()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public string Value { get; private set; } = null!;

    public static TenantIdentifier CreateInstance(string identifier)
    {
        return new TenantIdentifier(identifier);
    }
}