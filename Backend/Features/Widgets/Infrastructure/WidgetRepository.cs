using Backend.Core.Infrastructure.Repositories;
using Backend.Core.Infrastructure.Repositories.Serialisation;
using Backend.Core.Infrastructure.Tenancy;
using Backend.Features.Widgets.Application.Contracts;
using Backend.Features.Widgets.Domain.WidgetAggregate;
using Dapper;
using static Backend.Core.Infrastructure.Constants;

namespace Backend.Features.Widgets.Infrastructure;

internal class WidgetRepository : IWidgetRepository
{
    private readonly IDbConnection _connection;
    private readonly IGetTenantContext _context;

    public WidgetRepository(ITenantConnectionFactory factory, IGetTenantContext context)
    {
        _connection = factory.GetDbConnectionForTenant();
        _context = context;
    }

    public async Task<Widget?> Get(WidgetId widgetId, CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {TableWidgets} where {ColumnId} = @id";
        var result = await _connection.QuerySingleOrDefaultAsync<string>(sql, new
        {
            id = widgetId.Id
        });
        return JsonHelper.ToObject<Widget>(result);
    }

    public async Task<IEnumerable<Widget>> List(CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {TableWidgets}";
        var results = await _connection.QueryAsync<string>(sql, cancellationToken);
        return results
            .Select(result => JsonHelper.ToObject<Widget>(result)!)
            .ToList();
    }

    public async Task Insert(Widget widget, CancellationToken cancellationToken)
    {
        const string sql = $"insert into {TableWidgets} ({ColumnId}, {ColumnTenantId}, {ColumnData}) values (@id, @tenant_id, @data::jsonb)";
        var json = JsonHelper.ToJson(widget);
        await _connection.ExecuteAsync(sql, new
        {
            id = widget.Id.Id,
            tenant_id = _context.CurrentTenant,
            data = json
        });
    }

    public async Task Update(Widget widget, CancellationToken cancellationToken)
    {
        const string sql = $"update {TableWidgets} set {ColumnData} = @data::jsonb where {ColumnId} = @id";
        var result = await _connection.ExecuteAsync(sql, new
        {
            id = widget.Id.Id,
            data = JsonHelper.ToJson(widget)
        });
        if (result != 1)
        {
            throw new Exception("Record not updated");
        }
    }

}