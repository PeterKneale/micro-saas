using Backend.Modules.Infrastructure.Configuration;
using Backend.Modules.Infrastructure.Repositories.Serialisation;
using Backend.Modules.Settings.Infrastructure.Database;
using Npgsql;

namespace Backend.Modules.Settings.Infrastructure.Repository;

using static Constants;

internal class SettingsRepository : ISettingsRepository
{
    private readonly IDbConnection _connection;

    public SettingsRepository(ITenantConnectionFactory configuration)
    {
        _connection = configuration.GetDbConnectionForTenant();
    }

    public async Task Insert(Domain.SettingsAggregate.Settings settings, CancellationToken cancellationToken)
    {
        const string sql = $"insert into {Schema}.{TableSettings} ({ColumnData}) values (@data::jsonb)";
        var json = JsonHelper.ToJson(settings);
        await _connection.ExecuteAsync(sql, new
        {
            data = json
        });
    }

    public async Task Update(Domain.SettingsAggregate.Settings settings, CancellationToken cancellationToken)
    {
        const string sql = $"update {Schema}.{TableSettings} set {ColumnData} = @data::jsonb";
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
        const string sql = $"select {ColumnData} from {Schema}.{TableSettings}";
        var result = await _connection.QuerySingleOrDefaultAsync<string>(sql);
        return JsonHelper.ToObject<Domain.SettingsAggregate.Settings>(result);
    }
}