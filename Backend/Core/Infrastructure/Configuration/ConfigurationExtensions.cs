namespace Backend.Core.Infrastructure.Configuration;

public static class ConfigurationExtensions
{
    const string template = "Username={0};Password={1};Database={2};Host={3};Port={4};";

    public static string GetSystemConnectionString(this IConfiguration configuration)
    {
        var username = configuration["service:db:username"] ?? "postgres";
        var password = configuration["service:db:password"] ?? "password";
        var database = GetDatabase(configuration);
        var host = GetHost(configuration);
        var port = GetPort(configuration);
        return string.Format(template, username, password, database, host, port);
    }
    
    public static string GetConnectionStringForAdmin(this IConfiguration configuration)
    {
        var username =  "saas_admin";
        var password =  "password";
        var database = GetDatabase(configuration);
        var host = GetHost(configuration);
        var port = GetPort(configuration);
        return string.Format(template, username, password, database, host, port);
    }
    
    public static string GetConnectionStringForTenant(this IConfiguration configuration)
    {
        var username = "saas_tenant";
        var password = "password";
        var database = GetDatabase(configuration);
        var host = GetHost(configuration);
        var port = GetPort(configuration);
        return string.Format(template, username, password, database, host, port);
    }

    private static string GetHost(IConfiguration configuration) => 
        configuration["service:db:host"] ?? "localhost";
    
    private static string GetDatabase(IConfiguration configuration) => 
        configuration["service:db:database"] ?? "saas";
    
    private static string GetPort(IConfiguration configuration) => 
        configuration["service:db:port"] ?? "5432";

}