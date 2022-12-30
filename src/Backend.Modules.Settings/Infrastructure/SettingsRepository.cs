using Backend.Modules.Infrastructure.Repositories.Serialisation;

namespace Backend.Modules.Settings.Infrastructure;
using static Backend.Modules.Infrastructure.Database.Constants;

internal class SettingsRepository : ISettingsRepository
{
    private readonly IDbConnection _connection;

    public SettingsRepository(ITenantConnectionFactory factory)
    {
        _connection = factory.GetDbConnectionForTenant();
    }

    public async Task Insert(Domain.SettingsAggregate.Settings settings, CancellationToken cancellationToken)
    {
        const string sql = $"insert into {TableSettings} ({ColumnData}) values (@data::jsonb)";
        var json = JsonHelper.ToJson(settings);
        await _connection.ExecuteAsync(sql, new
        {
            data = json
        });
    }

    public async Task Update(Domain.SettingsAggregate.Settings settings, CancellationToken cancellationToken)
    {
        const string sql = $"update {TableSettings} set {ColumnData} = @data::jsonb";
        var result = await _connection.ExecuteAsync(sql, new
        {
            data = JsonHelper.ToJson(settings)
        });
        if (result != 1)
        {
            throw new Exception("Record not updated");
        }
    }

    public async Task<Domain.SettingsAggregate.Settings?> Get(CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {TableSettings}";
        var result = await _connection.QuerySingleOrDefaultAsync<string>(sql);
        return JsonHelper.ToObject<Domain.SettingsAggregate.Settings>(result);
    }
}