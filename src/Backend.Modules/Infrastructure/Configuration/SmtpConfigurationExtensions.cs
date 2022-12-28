namespace Backend.Modules.Infrastructure.Configuration;

public static class SmtpConfigurationExtensions
{
    public static string GetSmtpHost(this IConfiguration configuration) =>
        configuration["SMTP_HOST"] ?? "localhost";

    public static int GetSmtpPort(this IConfiguration configuration) =>
        int.Parse(configuration["SMTP_PORT"] ?? "1025");

    public static string? GetSmtpUsername(this IConfiguration configuration) =>
        configuration["SMTP_USERNAME"];

    public static string? GetSmtpPassword(this IConfiguration configuration) =>
        configuration["SMTP_PASSWORD"];
}