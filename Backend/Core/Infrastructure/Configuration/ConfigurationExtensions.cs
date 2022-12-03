namespace Backend.Core.Infrastructure.Configuration;

public static class ConfigurationExtensions
{
    public static string GetSystemConnectionString(this IConfiguration configuration) =>
        configuration["SystemConnectionString"];
    public static string GetConnectionStringForAdmin(this IConfiguration configuration) =>
        configuration["AdminConnectionString"];
    public static string GetConnectionStringForTenant(this IConfiguration configuration) =>
        configuration["TenantConnectionString"];
    
}