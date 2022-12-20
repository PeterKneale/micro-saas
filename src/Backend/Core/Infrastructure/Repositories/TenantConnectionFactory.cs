using Backend.Core.Infrastructure.Configuration;

namespace Backend.Core.Infrastructure.Repositories;

internal class TenantConnectionFactory : ITenantConnectionFactory
{
    private readonly NpgsqlConnection _tenant;

    public TenantConnectionFactory(IConfiguration configuration)
    {
        _tenant = new NpgsqlConnection(configuration.GetConnectionStringForTenant());
    }

    public IDbConnection GetDbConnectionForTenant() => _tenant;
}