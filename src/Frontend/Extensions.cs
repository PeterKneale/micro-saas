namespace Frontend;

internal static class Extensions
{
    private const string BackendKey = "backend";
    
    public static Uri GetRegistrationHttpUri(this IConfiguration configuration)
    {
        var key = "registration";
        var host = configuration[$"service:{key}:http:host"] ?? "localhost";
        var port = configuration[$"service:{key}:http:port"] ?? "8010";
        var protocol = configuration[$"service:{key}:http:protocol"] ?? "http";
        return new Uri(protocol + "://" + host + ":" + port + "/");
    }
    
    public static Uri GetServiceHttpUri(this IConfiguration configuration)
    {
        var host = configuration["API_HOST"] ?? "localhost";
        var port = configuration["API_PORT"] ?? "5000";
        return new Uri("http://" + host + ":" + port + "/");
    }

    public static Uri GetServiceGrpcUri(this IConfiguration configuration)
    {
        var host = configuration["API_HOST"] ?? "localhost";
        var port = configuration["API_PORT"] ?? "5001";
        return new Uri("http://" + host + ":" + port + "/");
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