namespace Modules.Widgets.Application.Exceptions;

internal class WidgetNotFoundException : NotFoundException
{
    public WidgetNotFoundException(Guid id) : base("car", id.ToString())
    {
    }
    public WidgetNotFoundException(string registration) : base("car", registration)
    {
    }
}