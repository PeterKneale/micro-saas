namespace Backend.Core.Infrastructure.Repositories;

public interface IAdminConnectionFactory
{
    IDbConnection GetDbConnectionForAdmin();
}