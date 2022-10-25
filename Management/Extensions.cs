using Microsoft.AspNetCore.Mvc;

namespace Management;

static class Extensions
{
    public static string GetBackendAddress(this IConfiguration configuration) =>
        Get(configuration, "BackendAddress");

    public static string GetEmail(this IConfiguration configuration) =>
        Get(configuration, "Email");

    public static string GetPassword(this IConfiguration configuration) =>
        Get(configuration, "Password");
    private static string Get(IConfiguration configuration, string key) =>
        configuration[key] ?? throw new Exception($"missing {key}");

    // see https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/security/authentication/cookie/samples/6.x/CookieSample/Extensions/UrlHelperExtensions.cs
    public static string? GetLocalUrl(this IUrlHelper urlHelper, string? localUrl)
    {
        return !urlHelper.IsLocalUrl(localUrl)
            ? urlHelper.Page("/Index")
            : localUrl;
    }
}