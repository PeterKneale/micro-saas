using Backend.Application.Contracts;
using Dapper;

namespace Backend.Infrastructure.Repositories;

internal class AdminRepository : IAdminRepository
{
    private readonly IDbConnection _connection;

    public AdminRepository(IConnectionFactory factory)
    {
        _connection = factory.GetDbConnectionForAdmin();
    }
    public async Task<int> Count(CancellationToken cancellationToken)
    {
        const string sql = "select count(1) from cars";
        return await _connection.QuerySingleAsync<int>(sql);
    }
}