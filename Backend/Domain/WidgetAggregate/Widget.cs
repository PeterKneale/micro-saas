namespace Backend.Domain.WidgetAggregate;

public class Widget
{
    private Widget(WidgetId id)
    {
        Id = id;
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

    public static Widget CreateInstance(WidgetId id)
    {
        return new(id);
    }
}