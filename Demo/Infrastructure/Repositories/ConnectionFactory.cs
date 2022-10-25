using Demo.Infrastructure.Configuration;

namespace Demo.Infrastructure.Repositories;

public interface IConnectionFactory
{
    IDbConnection GetConnection();
    IDbConnection GetAdminConnection();
}
internal class ConnectionFactory : IConnectionFactory
{
    private readonly NpgsqlConnection _tenant;
    private readonly NpgsqlConnection _admin;

    public ConnectionFactory(IConfiguration configuration)
    {
        _tenant = new NpgsqlConnection(configuration.GetTenantConnectionString());
        _admin = new NpgsqlConnection(configuration.GetAdminConnectionString());
    }

    public IDbConnection GetConnection() => _tenant;
    
    public IDbConnection GetAdminConnection() => _admin;
}