namespace Backend.Modules.Tenants.Domain.Common;

public class Email
{
    private Email(string value)
    {
        Value = value;
    }

    private Email()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public string Value { get; private set; } = null!;

    public static Email CreateInstance(string value)
    {
        return new Email(value);
    }
}