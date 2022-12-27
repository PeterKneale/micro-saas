using Backend.Modules.Widgets.Domain.WidgetAggregate;

namespace Backend.Modules.Widgets.Application.Contracts;

public interface IWidgetRepository
{
    Task<Widget?> Get(WidgetId id, CancellationToken cancellationToken);

    Task<IEnumerable<Widget>> List(CancellationToken cancellationToken);

    Task Insert(Widget widget, CancellationToken cancellationToken);

    Task Update(Widget widget, CancellationToken cancellationToken);
}