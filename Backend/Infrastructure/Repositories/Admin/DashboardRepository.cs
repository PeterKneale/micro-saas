using Backend.Application.Contracts.Admin;
using Dapper;
using static Backend.Infrastructure.Constants;

namespace Backend.Infrastructure.Repositories.Admin;

internal class DashboardRepository : IDashboardRepository
{
    private readonly IDbConnection _connection;

    public DashboardRepository(IConnectionFactory factory)
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