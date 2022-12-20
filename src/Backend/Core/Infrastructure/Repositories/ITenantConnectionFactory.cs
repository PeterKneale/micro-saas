namespace Backend.Core.Infrastructure.Repositories;

public interface ITenantConnectionFactory
{
    IDbConnection GetDbConnectionForTenant();
}