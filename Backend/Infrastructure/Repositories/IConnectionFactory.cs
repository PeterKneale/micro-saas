namespace Backend.Infrastructure.Repositories;

public interface IConnectionFactory
{
    IDbConnection GetDbConnectionForTenant();
    IDbConnection GetDbConnectionForAdmin();
}