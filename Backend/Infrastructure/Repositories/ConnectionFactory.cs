using Backend.Infrastructure.Configuration;

namespace Backend.Infrastructure.Repositories;

public interface IConnectionFactory
{
    IDbConnection GetDbConnectionForTenant();
    IDbConnection GetDbConnectionForAdmin();
}
internal class ConnectionFactory : IConnectionFactory
{
    private readonly NpgsqlConnection _tenant;
    private readonly NpgsqlConnection _admin;

    public ConnectionFactory(IConfiguration configuration)
    {
        _tenant = new NpgsqlConnection(configuration.GetConnectionStringForTenant());
        _admin = new NpgsqlConnection(configuration.GetConnectionStringForAdmin());
    }

    public IDbConnection GetDbConnectionForTenant() => _tenant;

    public IDbConnection GetDbConnectionForAdmin() => _admin;
}