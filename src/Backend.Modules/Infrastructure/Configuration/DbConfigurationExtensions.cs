namespace Backend.Modules.Infrastructure.Configuration;

public static class DbConfigurationExtensions
{
    private const string Template = "Username={0};Password={1};Database={2};Host={3};Port={4};";
    
    public static string GetSystemConnectionString(this IConfiguration configuration)
    {
        var username = configuration["DB_SYSTEM_USERNAME"] ?? "postgres";
        var password = configuration["DB_SYSTEM_PASSWORD"] ?? "password";
        var database = GetDbDatabase(configuration);
        var host = GetDbHost(configuration);
        var port = GetDbPort(configuration);
        return string.Format(Template, username, password, database, host, port);
    }

    public static string GetConnectionStringForAdmin(this IConfiguration configuration)
    {
        var username = configuration["DB_SYSTEM_USERNAME"] ?? "saas_admin";
        var password = configuration["DB_SYSTEM_PASSWORD"] ?? "password";
        var database = GetDbDatabase(configuration);
        var host = GetDbHost(configuration);
        var port = GetDbPort(configuration);
        return string.Format(Template, username, password, database, host, port);
    }

    public static string GetConnectionStringForTenant(this IConfiguration configuration)
    {
        var username = configuration["DB_SYSTEM_USERNAME"] ?? "saas_tenant";
        var password = configuration["DB_SYSTEM_PASSWORD"] ?? "password";
        var database = GetDbDatabase(configuration);
        var host = GetDbHost(configuration);
        var port = GetDbPort(configuration);
        return string.Format(Template, username, password, database, host, port);
    }

    private static string GetDbHost(IConfiguration configuration) =>
        configuration["DB_HOST"] ?? "localhost";

    private static string GetDbDatabase(IConfiguration configuration) =>
        configuration["DB_DATABASE"] ?? "saas";

    private static string GetDbPort(IConfiguration configuration) =>
        configuration["DB_PORT"] ?? "5432";
}