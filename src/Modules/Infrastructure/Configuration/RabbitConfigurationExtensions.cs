namespace Modules.Infrastructure.Configuration;

public static class RabbitConfigurationExtensions
{
    public static string GetRabbitUsername(this IConfiguration configuration) => 
        configuration["RABBIT_USERNAME"] ?? "rabbit";
    
    public static string GetRabbitPassword(this IConfiguration configuration) => 
        configuration["RABBIT_PASSWORD"] ?? "password";
    
    public static string GetRabbitHost(this IConfiguration configuration) => 
        configuration["RABBIT_HOST"] ?? "localhost";
    
    public static int GetRabbitPort(this IConfiguration configuration) => 
        int.Parse(configuration["RABBIT_PORT"] ?? "5672");

    public static string GetRabbitVirtualHost(this IConfiguration configuration) => 
        configuration["RABBIT_VHOST"] ?? "/";
}