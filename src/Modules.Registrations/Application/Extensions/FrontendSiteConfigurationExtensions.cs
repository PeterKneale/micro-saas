using Microsoft.Extensions.Configuration;

namespace Modules.Registrations.Application.Extensions;

public static class FrontendSiteConfigurationExtensions 
{
    public static Uri GetFrontendSiteUri(this IConfiguration configuration) =>
        new($"{configuration.GetProtocol()}://{configuration.GetHost()}:{configuration.GetPort()}", UriKind.Absolute);

    private static string GetHost(this IConfiguration configuration) =>
        configuration["FRONTEND_HOST"] ?? "localhost";

    private static int GetPort(this IConfiguration configuration) =>
        int.Parse(configuration["FRONTEND_PORT"] ?? "8020");

    private static string GetProtocol(this IConfiguration configuration) =>
        configuration["FRONTEND_PROTOCOL"] ?? "http";
}