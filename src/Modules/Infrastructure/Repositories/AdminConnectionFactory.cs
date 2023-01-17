namespace Modules.Infrastructure.Repositories;

internal class AdminConnectionFactory : IAdminConnectionFactory
{
    private readonly NpgsqlConnection _admin;

    public AdminConnectionFactory(IConfiguration configuration)
    {
        _admin = new NpgsqlConnection(configuration.GetConnectionStringForAdmin());
    }

    public IDbConnection GetDbConnectionForAdmin() => _admin;
}