namespace Modules.Widgets.Domain.WidgetAggregate;

public class WidgetId
{
    private WidgetId(Guid id)
    {
        Id = id;
    }

    private WidgetId()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public Guid Id { get; private set; }

    public static WidgetId CreateInstance(Guid? id = null)
    {
        return new WidgetId(id ?? Guid.NewGuid());
    }
}