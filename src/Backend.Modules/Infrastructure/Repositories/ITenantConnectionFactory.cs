namespace Backend.Modules.Infrastructure.Repositories;

public interface ITenantConnectionFactory
{
    IDbConnection GetDbConnectionForTenant();
}