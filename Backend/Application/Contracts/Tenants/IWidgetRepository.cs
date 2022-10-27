using Backend.Domain.WidgetAggregate;

namespace Backend.Application.Contracts.Tenants;

public interface IWidgetRepository
{
    Task<Widget?> Get(WidgetId id, CancellationToken cancellationToken);

    Task<IEnumerable<Widget>> List(CancellationToken cancellationToken);

    Task Insert(Widget widget, CancellationToken cancellationToken);

    Task Update(Widget widget, CancellationToken cancellationToken);
}