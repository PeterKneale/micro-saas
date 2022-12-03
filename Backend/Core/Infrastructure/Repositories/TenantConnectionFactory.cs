using Backend.Core.Infrastructure.Configuration;

namespace Backend.Core.Infrastructure.Repositories;

public interface ITenantConnectionFactory
{
    IDbConnection GetDbConnectionForTenant();
}

internal class TenantConnectionFactory : ITenantConnectionFactory
{
    private readonly NpgsqlConnection _tenant;

    public TenantConnectionFactory(IConfiguration configuration)
    {
        _tenant = new NpgsqlConnection(configuration.GetConnectionStringForTenant());
    }

    public IDbConnection GetDbConnectionForTenant() => _tenant;
}