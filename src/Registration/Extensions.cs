namespace Registration;

internal static class Extensions
{
    private const string BackendKey = "backend";
    
    public static Uri GetServiceHttpUri(this IConfiguration configuration)
    {
        var host = configuration[$"service:{BackendKey}:http:host"] ?? "localhost";
        var port = configuration[$"service:{BackendKey}:http:port"] ?? "5000";
        var protocol = configuration[$"service:{BackendKey}:http:protocol"] ?? "http";
        return new Uri(protocol + "://" + host + ":" + port + "/");
    }

    public static Uri GetServiceGrpcUri(this IConfiguration configuration)
    {
        var host = configuration[$"service:{BackendKey}:grpc:host"] ?? "localhost";
        var port = configuration[$"service:{BackendKey}:grpc:port"] ?? "5001";
        var protocol = configuration[$"service:{BackendKey}:grpc:protocol"] ?? "http";
        return new Uri(protocol + "://" + host + ":" + port + "/");
    }

    private static string Get(this IConfiguration configuration, string key) =>
        configuration[key] ?? throw new Exception($"missing {key}");

    private static int GetInt(this IConfiguration configuration, string key) =>
        int.Parse(configuration.Get(key));
}