namespace Backend.Modules.Infrastructure.Configuration;

public static class BackendConfigurationExtensions
{
    private const string Key = "backend";

    public static Uri GetServiceHttpUri(this IConfiguration configuration)
    {
        var host = configuration[$"service:{Key}:http:host"] ?? "localhost";
        var port = configuration[$"service:{Key}:http:port"] ?? "5000";
        var protocol = configuration[$"service:{Key}:http:protocol"] ?? "http";
        return new Uri(protocol + "://" + host + ":" + port + "/");
    }

    public static Uri GetServiceGrpcUri(this IConfiguration configuration)
    {
        var host = configuration[$"service:{Key}:grpc:host"] ?? "localhost";
        var port = configuration[$"service:{Key}:grpc:port"] ?? "5001";
        var protocol = configuration[$"service:{Key}:grpc:protocol"] ?? "http";
        return new Uri(protocol + "://" + host + ":" + port + "/");
    }
}