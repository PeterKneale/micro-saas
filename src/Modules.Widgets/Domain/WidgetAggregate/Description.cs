namespace Modules.Widgets.Domain.WidgetAggregate;

public class Description
{
    private Description(string value)
    {
        Value = value;
    }

    private Description()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public string Value { get; private set; } = null!;

    public static Description CreateInstance(string value)
    {
        return new Description(value);
    }
}