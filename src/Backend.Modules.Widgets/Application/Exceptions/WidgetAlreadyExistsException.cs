namespace Backend.Modules.Widgets.Application.Exceptions;

internal class WidgetAlreadyExistsException : BaseAlreadyExistsException
{
    public WidgetAlreadyExistsException(Guid id) : base("widget", id.ToString())
    {
    }
}