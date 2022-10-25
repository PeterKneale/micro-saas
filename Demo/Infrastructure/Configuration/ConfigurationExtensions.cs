namespace Demo.Infrastructure.Configuration;

public static class ConfigurationExtensions
{
    public static string GetSystemConnectionString(this IConfiguration configuration) =>
        configuration["SystemConnectionString"];
    public static string GetAdminConnectionString(this IConfiguration configuration) =>
        configuration["AdminConnectionString"];
    public static string GetTenantConnectionString(this IConfiguration configuration) =>
        configuration["TenantConnectionString"];
    
}