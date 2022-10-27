namespace Description;

internal static class Extensions
{
    public static string GetBackendAddress(this IConfiguration configuration) =>
        Get(configuration, "BackendAddress");
    private static string Get(IConfiguration configuration, string key) =>
        configuration[key] ?? throw new Exception($"missing {key}");
}