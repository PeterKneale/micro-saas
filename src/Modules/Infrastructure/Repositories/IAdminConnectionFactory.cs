namespace Modules.Infrastructure.Repositories;

public interface IAdminConnectionFactory
{
    IDbConnection GetDbConnectionForAdmin();
}