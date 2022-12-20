using Backend.Core.Exceptions;

namespace Backend.Features.Widgets.Application.Exceptions;

internal class WidgetAlreadyExistsException : BaseAlreadyExistsException
{
    public WidgetAlreadyExistsException(Guid id) : base("widget", id.ToString())
    {
    }
}