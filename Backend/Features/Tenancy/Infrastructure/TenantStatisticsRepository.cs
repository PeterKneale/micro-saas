using Backend.Core.Infrastructure.Repositories;
using Backend.Features.Tenancy.Application.Contracts;
using Dapper;
using static Backend.Core.Infrastructure.Constants;

namespace Backend.Features.Tenancy.Infrastructure;

internal class TenantStatisticsRepository : ITenantStatisticsRepository
{
    private readonly IDbConnection _connection;

    public TenantStatisticsRepository(IAdminConnectionFactory factory)
    {
        _connection = factory.GetDbConnectionForAdmin();
    }
    
    public async Task<int> CountTenants(CancellationToken cancellationToken)
    {
        const string sql = $"select count(1) from {TableTenants}";
        return await _connection.QuerySingleAsync<int>(sql);
    }

    public async Task<int> CountWidgets(CancellationToken cancellationToken)
    {
        const string sql = $"select count(1) from {TableWidgets}";
        return await _connection.QuerySingleAsync<int>(sql);
    }
}