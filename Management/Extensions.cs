using Microsoft.AspNetCore.Mvc;

namespace Management;

internal static class Extensions
{
    public static string GetEmail(this IConfiguration configuration) =>
        Get(configuration, "Email");

    public static string GetPassword(this IConfiguration configuration) =>
        Get(configuration, "Password");

    // see https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/security/authentication/cookie/samples/6.x/CookieSample/Extensions/UrlHelperExtensions.cs
    public static string? GetLocalUrl(this IUrlHelper urlHelper, string? localUrl)
    {
        return !urlHelper.IsLocalUrl(localUrl)
            ? urlHelper.Page("/Index")
            : localUrl;
    }
    
    public static Uri GetServiceHttpUri(this IConfiguration configuration, string key) =>
        configuration.GetServiceUri(key, "http");

    public static Uri GetServiceGrpcUri(this IConfiguration configuration, string key) =>
        configuration.GetServiceUri(key, "grpc");

    private static Uri GetServiceUri(this IConfiguration configuration, string key, string binding)
    {
        var host = Get(configuration, $"service:{key}:{binding}:host");
        var port = GetInt(configuration, $"service:{key}:{binding}:port");
        var protocol = configuration[$"service:{key}:{binding}:protocol"] ?? "http";
        return new Uri(protocol + "://" + host + ":" + port + "/");
    }

    private static string Get(this IConfiguration configuration, string key) =>
        configuration[key] ?? throw new Exception($"missing {key}");

    private static int GetInt(this IConfiguration configuration, string key) =>
        int.Parse(configuration.Get(key));
}