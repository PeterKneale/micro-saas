﻿namespace Frontend;

internal static class Extensions
{
    public static Uri GetServiceHttpUri(this IConfiguration configuration, string key) =>
        configuration.GetServiceUri(key, "http");

    public static Uri GetServiceGrpcUri(this IConfiguration configuration, string key) =>
        configuration.GetServiceUri(key, "grpc");

    private static Uri GetServiceUri(this IConfiguration configuration, string key, string binding)
    {
        var host = configuration[$"service:{key}:{binding}:host"] ?? "backend";
        var port = configuration[$"service:{key}:{binding}:port"] ?? "5001";
        var protocol = configuration[$"service:{key}:{binding}:protocol"] ?? "http";
        return new Uri(protocol + "://" + host + ":" + port + "/");
    }

    public static string GetEmail(this IConfiguration configuration) =>
        Get(configuration, "Email");

    public static string GetPassword(this IConfiguration configuration) =>
        Get(configuration, "Password");

    public static string GetTenant(this IConfiguration configuration) =>
        Get(configuration, "Tenant");
    
    // see https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/security/authentication/cookie/samples/6.x/CookieSample/Extensions/UrlHelperExtensions.cs
    public static string? GetLocalUrl(this IUrlHelper urlHelper, string? localUrl)
    {
        return !urlHelper.IsLocalUrl(localUrl)
            ? urlHelper.Page("/Index")
            : localUrl;
    }

    private static string Get(this IConfiguration configuration, string key) =>
        configuration[key] ?? throw new Exception($"missing {key}");
}