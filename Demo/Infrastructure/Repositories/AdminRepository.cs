using Dapper;
using Demo.Application.Contracts;

namespace Demo.Infrastructure.Repositories;

internal class AdminRepository : IAdminRepository
{
    private readonly IDbConnection _connection;

    public AdminRepository(IConnectionFactory factory)
    {
        _connection = factory.GetAdminConnection();
    }
    public async Task<int> Count(CancellationToken cancellationToken)
    {
        const string sql = "select count(1) from cars";
        return await _connection.QuerySingleAsync<int>(sql);
    }
}