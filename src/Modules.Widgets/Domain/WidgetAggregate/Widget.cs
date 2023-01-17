namespace Modules.Widgets.Domain.WidgetAggregate;

public class Widget
{
    private Widget(WidgetId id, Description? description)
    {
        Id = id;
        Description = description;
    }

    private Widget()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public WidgetId Id { get; private set; } = null!;

    public Description? Description { get; private set; }

    public void Update(Description registration)
    {
        Description = registration;
    }

    public static Widget CreateInstance(WidgetId id, Description? description) => 
        new(id, description);
}