namespace Backend.Modules.Widgets.Application.Exceptions;

internal class WidgetAlreadyExistsException : AlreadyExistsException
{
    public WidgetAlreadyExistsException(Guid id) : base("widget", id.ToString())
    {
    }
}