namespace Registration;

internal static class Extensions
{
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
}