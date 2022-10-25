using Backend.Application.Contracts.Admin;
using Dapper;

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
        const string sql = $"select count(1) from tenants";
        return await _connection.QuerySingleAsync<int>(sql);
    }

    public async Task<int> CountCars(CancellationToken cancellationToken)
    {
        const string sql = $"select count(1) from cars";
        return await _connection.QuerySingleAsync<int>(sql);
    }
}