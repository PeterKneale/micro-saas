namespace Backend.Modules.Widgets.Application.Exceptions;

internal class WidgetNotFoundException : BaseNotFoundException
{
    public WidgetNotFoundException(Guid id) : base("car", id.ToString())
    {
    }
    public WidgetNotFoundException(string registration) : base("car", registration)
    {
    }
}