namespace Backend.Modules.Infrastructure.Configuration;

public static class DbConfigurationExtensions
{
    private const string Template = "Username={0};Password={1};Database={2};Host={3};Port={4};";

    public static string GetSystemConnectionString(this IConfiguration configuration)
    {
        var username = configuration["service:db:username"] ?? "postgres";
        var password = configuration["service:db:password"] ?? "password";
        var database = GetDbDatabase(configuration);
        var host = GetDbHost(configuration);
        var port = GetDbPort(configuration);
        return string.Format(Template, username, password, database, host, port);
    }

    public static string GetConnectionStringForAdmin(this IConfiguration configuration)
    {
        var username = "saas_admin";
        var password = "password";
        var database = GetDbDatabase(configuration);
        var host = GetDbHost(configuration);
        var port = GetDbPort(configuration);
        return string.Format(Template, username, password, database, host, port);
    }

    public static string GetConnectionStringForTenant(this IConfiguration configuration)
    {
        var username = "saas_tenant";
        var password = "password";
        var database = GetDbDatabase(configuration);
        var host = GetDbHost(configuration);
        var port = GetDbPort(configuration);
        return string.Format(Template, username, password, database, host, port);
    }

    private static string GetDbHost(IConfiguration configuration) =>
        configuration["service:db:host"] ?? "localhost";

    private static string GetDbDatabase(IConfiguration configuration) =>
        configuration["service:db:database"] ?? "saas";

    private static string GetDbPort(IConfiguration configuration) =>
        configuration["service:db:port"] ?? "5432";
}