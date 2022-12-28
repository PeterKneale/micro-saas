namespace Registration;

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

    private static string Get(this IConfiguration configuration, string key) =>
        configuration[key] ?? throw new Exception($"missing {key}");

    private static int GetInt(this IConfiguration configuration, string key) =>
        int.Parse(configuration.Get(key));
}