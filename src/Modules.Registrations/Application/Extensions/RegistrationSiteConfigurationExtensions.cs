using Microsoft.Extensions.Configuration;

namespace Modules.Registrations.Application.Extensions;

public static class RegistrationSiteConfigurationExtensions 
{
    public static Uri GetRegistrationSiteUri(this IConfiguration configuration) =>
        new($"{configuration.GetProtocol()}://{configuration.GetHost()}:{configuration.GetPort()}", UriKind.Absolute);

    private static string GetHost(this IConfiguration configuration) =>
        configuration["REGISTRATION_HOST"] ?? "localhost";

    private static int GetPort(this IConfiguration configuration) =>
        int.Parse(configuration["REGISTRATION_PORT"] ?? "8010");

    private static string GetProtocol(this IConfiguration configuration) =>
        configuration["REGISTRATION_PROTOCOL"] ?? "http";
}