using Grpc.Core;

namespace Backend.FunctionalTests;

internal static class MetaDataBuilder
{
    public static Metadata WithTenant() => new()
    {
        {
            "tenant", Guid.NewGuid().ToString()
        }
    };
    public static Metadata WithTenant(string id) => new()
    {
        {
            "tenant", id
        }
    };
}