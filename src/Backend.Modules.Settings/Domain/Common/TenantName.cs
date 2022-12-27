namespace Backend.Modules.Settings.Domain.Common;

public class TenantName
{
    private TenantName(string value)
    {
        Value = value;
    }

    private TenantName()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public string Value { get; private set; } = null!;

    public static TenantName CreateInstance(string value)
    {
        return new TenantName(value);
    }
}