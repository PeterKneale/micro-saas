using Modules.Statistics.Application.Contracts;

namespace Modules.Statistics.Infrastructure;

internal class TenantStatisticsRepository : ITenantStatisticsRepository
{
    private readonly IDbConnection _connection;

    public TenantStatisticsRepository(IAdminConnectionFactory factory)
    {
        _connection = factory.GetDbConnectionForAdmin();
    }
    
    public async Task<int> CountTenants(CancellationToken cancellationToken)
    {
        const string sql = $"select 1;";
        return await _connection.QuerySingleAsync<int>(sql);
    }

    public async Task<int> CountWidgets(CancellationToken cancellationToken)
    {
        const string sql = $"select 1;";
        return await _connection.QuerySingleAsync<int>(sql);
    }
}