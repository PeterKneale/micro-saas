using Backend.Core.Infrastructure.Tenancy;
using Backend.Features.Tenancy.Domain.SettingsAggregate;
using static Backend.Core.Infrastructure.Constants;

namespace Backend.Features.Tenancy.Infrastructure;

internal class SettingsRepository : ISettingsRepository
{
    private readonly IGetTenantContext _context;
    private readonly IDbConnection _connection;

    public SettingsRepository(ITenantConnectionFactory factory, IGetTenantContext context)
    {
        _context = context;
        _connection = factory.GetDbConnectionForTenant();
    }

    public async Task Insert(Settings settings, CancellationToken cancellationToken)
    {
        const string sql = $"insert into {TableSettings} ({ColumnTenantId}, {ColumnData}) values (@id, @data::jsonb)";
        var json = JsonHelper.ToJson(settings);
        await _connection.ExecuteAsync(sql, new
        {
            id = _context.CurrentTenant,
            data = json
        });
    }

    public async Task Update(Settings settings, CancellationToken cancellationToken)
    {
        const string sql = $"update {TableSettings} set {ColumnData} = @data::jsonb where {ColumnTenantId} = @id";
        var result = await _connection.ExecuteAsync(sql, new
        {
            id = _context.CurrentTenant,
            data = JsonHelper.ToJson(settings)
        });
        if (result != 1)
        {
            throw new Exception("Record not updated");
        }
    }

    public async Task<Settings?> Get(CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {TableSettings} where {ColumnTenantId} = @id";
        var result = await _connection.QuerySingleOrDefaultAsync<string>(sql, new
        {
            id = _context.CurrentTenant
        });
        return JsonHelper.ToObject<Settings>(result);
    }
}