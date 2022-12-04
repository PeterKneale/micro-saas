namespace Backend.Core.Infrastructure.Configuration;

public static class ConfigurationExtensions
{
    const string template = "Username={0};Password={1};Database={2};Host={3};Port={4};";

    public static string GetSystemConnectionString(this IConfiguration configuration) => GetConnectionString(configuration);
    public static string GetConnectionStringForAdmin(this IConfiguration configuration) => GetConnectionString(configuration);
    public static string GetConnectionStringForTenant(this IConfiguration configuration) => GetConnectionString(configuration);
    private static string GetConnectionString(IConfiguration configuration)
    {
        var username = configuration["service:db:username"] ?? "postgres";
        var password = configuration["service:db:password"] ?? "password";
        var database = configuration["service:db:database"] ?? "saas";
        var host = configuration["service:db:host"] ?? "localhost";
        var port = configuration["service:db:port"] ?? "5432";
        return string.Format(template, username, password, database, host, port);
    }
}